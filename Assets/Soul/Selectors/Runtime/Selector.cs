using System;
using System.Threading;
using Pancake.MobileInput;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Soul.Selectors.Runtime
{
    [Serializable]
    public class Selector
    {
        [SerializeField] private LayerMask selectableLayers = Physics.DefaultRaycastLayers;
        [SerializeField] private float waitForDrag = 0.1f;
        [SerializeField] private bool useMultipleCallbacks = false;
        [SerializeField] private UnityEvent<RaycastHit, ESelectionState> onSelectionEvent;

        private Action _onOverUI;
        private Transform _currentSelection;
        private bool _canSelect = true;
        private bool _selectProcessRunning = false;
        private EventSystem _eventSystem;
        private Camera _mainCamera;
        private CancellationToken _token;
        private RaycastHit _lastHit;
        private Vector3 _dragStartPosition;
        private Vector3 _dragStartWorldPosition;
        private bool _isDragging = false;

        // Efficient short-term caching
        private Transform _lastCheckedTransform;
        private ISelectCallBackReceiver[] _cachedCallbacks;

        public void Initialize(Camera camera, EventSystem eventSystemInstance, Action onOverUI,
            CancellationToken cts)
        {
            _mainCamera = camera;
            _eventSystem = eventSystemInstance;
            _onOverUI = onOverUI;
            _token = cts;
        }

        public void Subscribe()
        {
            TouchInput.OnStartDrag += HandleStartDrag;
            TouchInput.OnStopDrag += HandleStopDrag;
            TouchInput.OnUpdateDrag += HandleUpdateDrag;
            TouchInput.OnFingerDown += HandleFingerDown;
        }

        public float z;

        private void HandleUpdateDrag(Vector3 startPosition, Vector3 currentPosition, Vector3 offset, Vector3 delta)
        {
            if (_isDragging && _currentSelection != null)
            {
                if (ScreenRayCast(currentPosition, out var raycastHit))
                {
                    var isInside = raycastHit.transform == _currentSelection;
                    var callbacks = GetCallbacks(_currentSelection);
                    foreach (var callback in callbacks)
                    {
                        callback.OnUpdateDrag(_lastHit, isInside, raycastHit.point, delta);
                    }
                }
            }
        }

        private bool ScreenRayCast(Vector3 currentPosition, out RaycastHit hit)
        {
            var ray = _mainCamera.ScreenPointToRay(currentPosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayers);
        }

        public void Unsubscribe()
        {
            TouchInput.OnStartDrag -= HandleStartDrag;
            TouchInput.OnStopDrag -= HandleStopDrag;
            TouchInput.OnUpdateDrag -= HandleUpdateDrag;
            TouchInput.OnFingerDown -= HandleFingerDown;
            ClearCache();
        }

        private void HandleStartDrag(Vector3 position, bool isLongTap)
        {
            _canSelect = false;
            _isDragging = true;
            _dragStartPosition = position;

            // Perform raycast to get the world position at the start of the drag
            if (ScreenRayCast(position, out var hit))
            {
                _dragStartWorldPosition = hit.point;
                _lastHit = hit;
            }
        }

        private void HandleStopDrag(Vector3 position, Vector3 delta)
        {
            _canSelect = true;
            _isDragging = false;

            if (_currentSelection != null)
            {
                if (ScreenRayCast(position, out var hit))
                {
                    var isInside = hit.transform == _currentSelection;
                    var callbacks = GetCallbacks(_currentSelection);
                    foreach (var callback in callbacks)
                    {
                        callback.OnDragEnd(_lastHit, isInside, hit.point);
                    }
                }
                
            }
        }

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
                await Awaitable.WaitForSecondsAsync(waitForDrag, _token);

                if (!_isDragging)
                {
                    PerformSelection(screenPoint);
                }
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

            _lastHit = hit;
#if UNITY_EDITOR
            if (_currentSelection != null) UnityEditor.EditorGUIUtility.PingObject(_currentSelection.gameObject);
#endif
        }

        private void HandleReselection(ISelectCallBackReceiver[] callbacks, RaycastHit hit)
        {
            foreach (var callback in callbacks) callback.OnReselected(hit);
            onSelectionEvent.Invoke(hit, ESelectionState.Reselected);
        }

        private void HandleNewSelection(ISelectCallBackReceiver[] callbacks, RaycastHit hit)
        {
            if (_currentSelection != null) HandleDeselection(hit);

            _currentSelection = hit.transform;
            foreach (var callback in callbacks) callback.OnSelected(hit);
            onSelectionEvent.Invoke(hit, ESelectionState.Selected);
        }

        private void HandleDeselection(RaycastHit hit)
        {
            var deselectedCallbacks = GetCallbacks(_currentSelection);
            foreach (var callback in deselectedCallbacks) callback.OnDeselected(_lastHit, hit);
            onSelectionEvent.Invoke(hit, ESelectionState.Deselected);
            _currentSelection = null;
        }

        private ISelectCallBackReceiver[] GetCallbacks(Transform transform)
        {
            if (_lastCheckedTransform == transform && _cachedCallbacks != null) return _cachedCallbacks;

            _lastCheckedTransform = transform;
            _cachedCallbacks = useMultipleCallbacks
                ? transform.GetComponentsInChildren<ISelectCallBackReceiver>()
                : new[] { transform.GetComponent<ISelectCallBackReceiver>() };

            _cachedCallbacks = _cachedCallbacks[0] != null ? _cachedCallbacks : Array.Empty<ISelectCallBackReceiver>();
            return _cachedCallbacks;
        }

        private void ClearCache()
        {
            _lastCheckedTransform = null;
            _cachedCallbacks = null;
        }
    }
}