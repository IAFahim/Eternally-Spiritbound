using System.Collections.Generic;
using _Root.Scripts.Game.FocusProvider.Runtime;
using Soul.Interactables.Runtime;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    /// <summary>
    /// Manages interactions between game objects by detecting overlaps and handling hover events.
    /// </summary>
    public class InteractorComponent : MonoBehaviour, IInteractor
    {
        [SerializeField] private IntervalTicker ticker;
        [SerializeField] private OverlapCheckedNonAlloc interactableOverlapChecked;

        public bool Focused => _focusReference.IsFocused;
        public GameObject GameObject => gameObject;

        private IFocus _focusReference;
        private readonly Dictionary<Collider, IInteractable> _activeInteractable = new();
        private readonly List<Collider> _collidersToRemove = new();

        private void Awake()
        {
            _focusReference = GetComponent<IFocus>();
#if UNITY_EDITOR
            ValidateComponents();
#endif
        }

        private void Start()
        {
            interactableOverlapChecked.Initialize();
        }

        private void Update()
        {
            ProcessOverlappingColliders();
        }

        private void FixedUpdate()
        {
            if (ticker.TryTick())
            {
                interactableOverlapChecked.Perform();
            }
        }

        private void OnDisable()
        {
            CleanupAllInteractions();
        }

        private void ProcessOverlappingColliders()
        {
            if (!GetCurrentColliders(out HashSet<Collider> currentColliders)) return;

            HandleNewColliders(currentColliders);
            HandleRemovedColliders(currentColliders);
        }

        private bool GetCurrentColliders(out HashSet<Collider> currentColliders)
        {
            int colliderCount = interactableOverlapChecked.GetColliders(out var colliders);
            if (colliderCount == 0)
            {
                currentColliders = null;
                return false;
            }

            currentColliders = new HashSet<Collider>();
            for (var i = 0; i < colliderCount; i++)
            {
                var foundCollider = colliders[i];
                if (foundCollider != null) currentColliders.Add(foundCollider);
            }

            return true;
        }

        private void HandleNewColliders(HashSet<Collider> currentColliders)
        {
            foreach (var item in currentColliders)
            {
                if (!_activeInteractable.ContainsKey(item))
                {
                    if (item.TryGetComponent(out IInteractable interactable))
                    {
                        _activeInteractable.Add(item, interactable);
                        interactable.OnInteractableDetected(this);
                    }
                }
            }
        }

        private void HandleRemovedColliders(HashSet<Collider> currentColliders)
        {
            _collidersToRemove.Clear();

            foreach (var item in _activeInteractable.Keys)
            {
                if (!currentColliders.Contains(item)) _collidersToRemove.Add(item);
            }

            foreach (var item in _collidersToRemove)
            {
                if (_activeInteractable.TryGetValue(item, out var interactable))
                {
                    interactable.OnInteractableDetectionLost(this);
                    _activeInteractable.Remove(item);
                }
            }
        }

        private void CleanupAllInteractions()
        {
            foreach (var interactable in _activeInteractable.Values)
            {
                if (interactable != null)
                {
                    interactable.OnInteractableDetectionLost(this);
                }
            }

            _activeInteractable.Clear();
        }

#if UNITY_EDITOR
        private void ValidateComponents()
        {
            if (_focusReference == null)
            {
                Debug.LogError($"Missing IFocus component on {gameObject.name}");
            }

            if (interactableOverlapChecked == null)
            {
                Debug.LogError($"Missing OverlapCheckedNonAlloc reference on {gameObject.name}");
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (interactableOverlapChecked != null)
            {
                interactableOverlapChecked.DrawGizmos(Color.red, Color.green);
            }
        }
#endif
    }
}