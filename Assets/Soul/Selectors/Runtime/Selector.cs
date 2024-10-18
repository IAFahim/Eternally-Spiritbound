using System;
using System.Threading;
using Pancake.MobileInput;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Soul.Selectors.Runtime
{
    [Serializable]
    public class Selector<T>
    {
        [SerializeField] private LayerMask selectableLayers = Physics.DefaultRaycastLayers;
        [SerializeField] private float waitForDrag = 0.1f;
        [SerializeField] private bool useMultipleCallbacks = false;
        [SerializeField] private UnityEvent<RaycastHit, ESelectionState> onSelectionEvent;

        private T _info;
        private Action _onOverUI;
        private Transform _currentSelection;
        private bool _canSelect = true;
        private bool _selectProcessRunning = false;
        private EventSystem _eventSystem;
        private Camera _mainCamera;
        private CancellationTokenSource _cts;

        // Efficient short-term caching
        private Transform _lastCheckedTransform;
        private ISelectorBase<T>[] _cachedCallbacks;

        public void Initialize(T info, Camera camera, EventSystem eventSystemInstance, Action onOverUI,
            CancellationTokenSource cts)
        {
            _info = info;
            _mainCamera = camera;
            _eventSystem = eventSystemInstance;
            _onOverUI = onOverUI;
            _cts = cts;
        }

        public void Subscribe()
        {
            TouchInput.OnStartDrag += HandleStartDrag;
            TouchInput.OnStopDrag += HandleStopDrag;
            TouchInput.OnFingerDown += HandleFingerDown;
        }

        public void Unsubscribe()
        {
            TouchInput.OnStartDrag -= HandleStartDrag;
            TouchInput.OnStopDrag -= HandleStopDrag;
            TouchInput.OnFingerDown -= HandleFingerDown;
            _cts.Cancel();
            _cts.Dispose();
            ClearCache();
        }

        private void HandleStartDrag(Vector3 position, bool isLongTap) => _canSelect = false;

        private void HandleStopDrag(Vector3 arg1, Vector3 arg2) => _canSelect = true;

        private async void HandleFingerDown(Vector3 screenPoint)
        {
            if (_eventSystem.IsPointerOverGameObject() || _eventSystem.currentSelectedGameObject != null)
            {
                _onOverUI?.Invoke();
                return;
            }

            if (_selectProcessRunning) return;
            _selectProcessRunning = true;

            try
            {
                await Awaitable.WaitForSecondsAsync(waitForDrag, _cts.Token);
                PerformSelection(screenPoint);
            }
            catch (OperationCanceledException)
            {
                // Operation was canceled, we can safely ignore this
            }
            finally
            {
                _selectProcessRunning = false;
            }
        }

        public void PerformSelection(Vector3 screenPoint)
        {
            if (!_canSelect) return;

            var ray = _mainCamera.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, selectableLayers))
            {
                var hitTransform = hit.transform;
                var selectionCallbacks = GetCallbacks(hitTransform);
                if (selectionCallbacks.Length > 0)
                {
                    if (_currentSelection == hitTransform)
                    {
                        HandleReselection(selectionCallbacks, hit);
                    }
                    else
                    {
                        HandleNewSelection(selectionCallbacks, hit);
                    }
                }
                else if (_currentSelection != null)
                {
                    HandleDeselection(hit);
                }
            }
            else if (_currentSelection != null) HandleDeselection(hit);

#if UNITY_EDITOR
            if (_currentSelection != null) UnityEditor.EditorGUIUtility.PingObject(_currentSelection.gameObject);
#endif
        }

        private void HandleReselection(ISelectorBase<T>[] callbacks, RaycastHit hit)
        {
            foreach (var callback in callbacks) callback.OnReselected(_info);
            onSelectionEvent.Invoke(hit, ESelectionState.Reselected);
        }

        private void HandleNewSelection(ISelectorBase<T>[] callbacks, RaycastHit hit)
        {
            if (_currentSelection != null) HandleDeselection(hit);

            _currentSelection = hit.transform;
            foreach (var callback in callbacks) callback.OnSelected(_info);
            onSelectionEvent.Invoke(hit, ESelectionState.Selected);
        }

        private void HandleDeselection(RaycastHit hit)
        {
            var deselectedCallbacks = GetCallbacks(_currentSelection);
            foreach (var callback in deselectedCallbacks) callback.OnDeselected(hit, _info);
            onSelectionEvent.Invoke(hit, ESelectionState.Deselected);
            _currentSelection = null;
        }

        private ISelectorBase<T>[] GetCallbacks(Transform transform)
        {
            if (_lastCheckedTransform == transform && _cachedCallbacks != null) return _cachedCallbacks;

            _lastCheckedTransform = transform;
            _cachedCallbacks = useMultipleCallbacks
                ? transform.GetComponentsInChildren<ISelectorBase<T>>()
                : new[] { transform.GetComponent<ISelectorBase<T>>() };

            _cachedCallbacks = _cachedCallbacks[0] != null ? _cachedCallbacks : Array.Empty<ISelectorBase<T>>();
            return _cachedCallbacks;
        }

        private void ClearCache()
        {
            _lastCheckedTransform = null;
            _cachedCallbacks = null;
        }
    }
}