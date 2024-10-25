using System;
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
        private readonly List<InteractableInfo> _interactableInfos = new List<InteractableInfo>();

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


        private void OnDisable()
        {
            CleanupAllInteractions();
        }

        private void CleanupAllInteractions()
        {
            foreach (var interactableInfo in _interactableInfos)
            {
                interactableInfo.Interactable.OnInteractableDetectionLost(this);
            }

            _interactableInfos.Clear();
        }

        private void Update()
        {
            AddNewEntryToList();
            CleanUpBasedOnDistance();
        }

        private void AddNewEntryToList()
        {
            if (0 < interactableOverlapChecked.GetColliders(out var colliders))
            {
                foreach (var currentCollider in colliders)
                {
                    if (currentCollider == null) continue;
                    var parentIfNotFoundSelf = currentCollider.transform.parent ?? currentCollider.transform;
                    if (InteractableInfo.ListContains(_interactableInfos, currentCollider)) continue;
                    if (!parentIfNotFoundSelf.TryGetComponent(out IInteractable interactable)) continue;
                    var interactableInfo = new InteractableInfo(parentIfNotFoundSelf, currentCollider, interactable);
                    _interactableInfos.Add(interactableInfo);
                    interactable.OnInteractableDetected(this);
                }
            }
        }

        private void CleanUpBasedOnDistance()
        {
            for (int i = _interactableInfos.Count - 1; i >= 0; i--)
            {
                var interactableInfo = _interactableInfos[i];
                if (!interactableInfo.IsActiveInRange(transform.position, interactableOverlapChecked.sphereRadius))
                {
                    interactableInfo.Interactable.OnInteractableDetectionLost(this);
                    _interactableInfos.RemoveAt(i);
                }
            }
        }

        private void FixedUpdate()
        {
            if (ticker.TryTick())
            {
                interactableOverlapChecked.Perform();
            }
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

    struct InteractableInfo : IEquatable<InteractableInfo>
    {
        public readonly Transform Transform;
        public readonly GameObject GameObject;
        public readonly int Hash;
        public readonly float BoundsExtentsMagnitude;
        public IInteractable Interactable;

        public InteractableInfo(Transform transform, Collider collider, IInteractable interactable)
        {
            Transform = transform;
            GameObject = Transform.gameObject;
            Hash = Transform.GetInstanceID();
            BoundsExtentsMagnitude = collider.bounds.extents.magnitude;
            Interactable = interactable;
        }

        public bool IsActiveInRange(Vector3 position, float radius)
        {
            return GameObject.activeSelf &&
                   Vector3.Distance(position, Transform.position) < radius + BoundsExtentsMagnitude;
        }

        public override int GetHashCode() => Hash;

        public bool Equals(Collider collider)
        {
            return Hash == (collider.transform.parent ?? collider.transform).GetInstanceID();
        }

        public static bool ListContains(List<InteractableInfo> interactableInfos, Collider checkCollider)
        {
            foreach (var interactableInfo in interactableInfos)
            {
                if (interactableInfo.Equals(checkCollider)) return true;
            }

            return false;
        }

        public bool Equals(InteractableInfo other)
        {
            return Hash == other.Hash;
        }

        public override bool Equals(object obj)
        {
            return obj is InteractableInfo other && Equals(other);
        }
    }
}