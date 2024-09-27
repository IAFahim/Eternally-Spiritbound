using System;
using System.Collections.Generic;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupDetectHandlerBase<T> : PickupHandlerBase<PickupContainer<T>>
    {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private LayerMask layerMask;
        private readonly List<PickupContainer<T>> detectedControllers = new();
        private readonly List<PickupContainer<T>> checkIfCanBeAddedControllers = new();
        public Func<PickupContainer<T>, bool> HaveSpaceInInventory;

        public override void Handle(PickupContainer<T> responsibility)
        {
            responsibility.transform.gameObject.SetActive(true);
            detectedControllers.Add(responsibility);
        }

        public override void Process()
        {
            for (var i = detectedControllers.Count - 1; i >= 0; i--) InRangeChangeList(i);
            for (var i = checkIfCanBeAddedControllers.Count - 1; i >= 0; i--)
            {
                var pickupContainer = checkIfCanBeAddedControllers[i];
                if (CheckCanBeAdded(pickupContainer))
                {
                    checkIfCanBeAddedControllers.RemoveAt(i);
                    HandleNext(pickupContainer);
                }
                else detectedControllers.Add(pickupContainer);
            }
        }

        private void InRangeChangeList(int i)
        {
            var pickupContainer = detectedControllers[i];
            if (Physics.CheckSphere(pickupContainer.startPosition, detectionRadius, layerMask))
            {
                checkIfCanBeAddedControllers.Add(pickupContainer);
                detectedControllers.RemoveAt(i);
            }
        }


        public override void Dispose()
        {
            detectedControllers.Clear();
        }


        public bool CheckCanBeAdded(PickupContainer<T> pickupContainer)
        {
            var colliders = new Collider[1];
            Physics.OverlapSphereNonAlloc(pickupContainer.startPosition, detectionRadius, colliders, layerMask);
            var transform = colliders[0].transform;
            pickupContainer.otherTransform = transform;
            return HaveSpaceInInventory.Invoke(pickupContainer);
        }


#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.green;
        public override void OnDrawGizmos()
        {
            if (detectedControllers == null) return;
            Gizmos.color = gizmoColor;
            foreach (var controller in detectedControllers)
                Gizmos.DrawWireSphere(controller.transform.position, detectionRadius);
        }
#endif
    }
}