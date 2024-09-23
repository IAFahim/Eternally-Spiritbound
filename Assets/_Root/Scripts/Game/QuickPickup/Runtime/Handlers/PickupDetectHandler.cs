using System;
using System.Collections.Generic;
using Soul2.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupDetectHandler<T> : PickupHandler<T>
    {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private LayerMask layerMask;
        private readonly List<PickupContainer<T>> controllers = new();
        public override void Handle(PickupContainer<T> responsibility)
        {
            responsibility.transform.gameObject.SetActive(true);
            controllers.Add(responsibility);
        }

        public override void Process()
        {
            for (var i = controllers.Count - 1; i >= 0; i--)
            {
                var pickController = controllers[i];
                if (!TryActive(pickController)) continue;
                HandleNext(pickController);
                controllers.RemoveAt(i);
            }
        }

        public override void Clear()
        {
            controllers.Clear();
        }

        private bool TryActive(PickupContainer<T> pickupContainer)
        {
            if (!Physics.CheckSphere(pickupContainer.startPosition, detectionRadius, layerMask)) return false;
            var colliders = new Collider[1];
            Physics.OverlapSphereNonAlloc(pickupContainer.startPosition, detectionRadius, colliders, layerMask);
            pickupContainer.otherTransform = colliders[0].transform;
            return true;
        }

#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.green;
        public override void OnDrawGizmos()
        {
            if (controllers == null) return;
            Gizmos.color = gizmoColor;
            foreach (var controller in controllers)
                Gizmos.DrawWireSphere(controller.transform.position, detectionRadius);
        }
#endif
    }
}