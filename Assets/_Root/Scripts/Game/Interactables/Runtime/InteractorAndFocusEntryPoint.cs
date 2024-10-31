using System;
using System.Collections.Generic;
using Soul.Interactables.Runtime;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    /// <summary>
    /// Manages interactions between game objects by detecting overlaps and handling hover events.
    /// </summary>
    [DisallowMultipleComponent]
    [SelectionBase]
    public class InteractorAndFocusEntryPoint : FocusEntryPointComponent
    {
        [SerializeField] private IntervalTicker ticker;
        [SerializeField] private OverlapNonAlloc interactableOverlapChecked;

        private readonly List<InteractableInfo> _interactableInfos = new();

        private void Start()
        {
            interactableOverlapChecked.Initialize(transform);
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
            int interactableCount = interactableOverlapChecked.GetColliders(out var colliders);
            if (0 == interactableCount) return;
            for (var i = 0; i < interactableCount; i++)
            {
                var currentCollider = colliders[i];
                if (currentCollider == null) continue;
                var root = currentCollider.transform.root;
                if (InteractableInfo.ListContains(_interactableInfos, root)) continue;
                if (!root.TryGetComponent(out IInteractable interactable)) continue;
                _interactableInfos.Add(new InteractableInfo(root, currentCollider, interactable));
                interactable.OnInteractableDetected(this);
            }
        }

        private void CleanUpBasedOnDistance()
        {
            for (int i = _interactableInfos.Count - 1; i >= 0; i--)
            {
                var interactableInfo = _interactableInfos[i];
                if (!interactableInfo.IsActiveInRange(transform.position,
                        interactableOverlapChecked.config.sphereRadius))
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
        public readonly int Hash;
        public readonly GameObject GameObject;
        public readonly float BoundsExtentsMagnitude;
        public IInteractable Interactable;

        public InteractableInfo(Transform transform, Collider collider, IInteractable interactable)
        {
            Transform = transform;
            Hash = Transform.GetInstanceID();
            GameObject = Transform.gameObject;
            BoundsExtentsMagnitude = collider.bounds.extents.magnitude;
            Interactable = interactable;
        }

        public bool IsActiveInRange(Vector3 position, float radius)
        {
            return GameObject.activeSelf &&
                   Vector3.Distance(position, Transform.position) < radius + BoundsExtentsMagnitude;
        }

        public override int GetHashCode() => Hash;

        public static bool ListContains(List<InteractableInfo> interactableInfos, Transform rootTransform)
        {
            foreach (var interactableInfo in interactableInfos)
            {
                if (interactableInfo.Transform == rootTransform.transform) return true;
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