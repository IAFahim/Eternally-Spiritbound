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
    public class InteractableActivator : MonoBehaviour
    {
        [SerializeField] private IntervalTicker ticker;
        [SerializeField] private OverlapNonAlloc interactableOverlapChecked;
        private readonly List<InteractableInfo> _interactableInfos = new();
        private IInteractorEntryPoint _interactorEntryPoint;

        private void Awake() => _interactorEntryPoint = GetComponent<IInteractorEntryPoint>();
        private void Start() => interactableOverlapChecked.Initialize(transform);

        private void OnDisable()
        {
            CleanupAllInteractions();
        }

        private void CleanupAllInteractions()
        {
            foreach (var interactableInfo in _interactableInfos)
            {
                interactableInfo.InteractableEntryPoint.OnInteractableDetectionLost(_interactorEntryPoint);
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
                if (!root.TryGetComponent(out IInteractableEntryPoint interactable)) continue;
                _interactableInfos.Add(new InteractableInfo(root, currentCollider, interactable));
                interactable.OnInteractableDetected(_interactorEntryPoint);
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
                    interactableInfo.InteractableEntryPoint.OnInteractableDetectionLost(_interactorEntryPoint);
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
            interactableOverlapChecked.DrawGizmos(Color.red, Color.green);
        }
#endif
    }

    readonly struct InteractableInfo : IEquatable<InteractableInfo>
    {
        private readonly Transform _transform;
        private readonly int _hash;
        private readonly GameObject _gameObject;
        private readonly float _boundsExtentsMagnitude;
        public readonly IInteractableEntryPoint InteractableEntryPoint;

        public InteractableInfo(Transform transform, Collider collider, IInteractableEntryPoint interactableEntryPoint)
        {
            _transform = transform;
            _hash = _transform.GetInstanceID();
            _gameObject = _transform.gameObject;
            _boundsExtentsMagnitude = collider.bounds.extents.magnitude;
            InteractableEntryPoint = interactableEntryPoint;
        }

        public bool IsActiveInRange(Vector3 position, float radius)
        {
            return _gameObject.activeSelf &&
                   Vector3.Distance(position, _transform.position) < radius + _boundsExtentsMagnitude;
        }

        public override int GetHashCode() => _hash;

        public static bool ListContains(List<InteractableInfo> interactableInfos, Transform rootTransform)
        {
            foreach (var interactableInfo in interactableInfos)
            {
                if (interactableInfo._transform == rootTransform.transform) return true;
            }

            return false;
        }

        public bool Equals(InteractableInfo other) => _hash == other._hash;

        public override bool Equals(object obj) => obj is InteractableInfo other && Equals(other);
    }
}