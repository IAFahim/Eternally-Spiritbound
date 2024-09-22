using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.PickableItems.Interaction
{
    [Serializable]
    public class PickController
    {
        public Transform transform;
        public Vector3 startPosition;
        public int amount;
        public Collider collider;
        public bool targetSet;

        public PickController(Transform transform, int staringAmount)
        {
            this.transform = transform;
            startPosition = transform.position;
            amount = staringAmount;
        }

        public bool TryActive(float detectionRadius, LayerMask layerMask, out PickController pickController)
        {
            if (Physics.CheckSphere(startPosition, detectionRadius, layerMask))
            {
                pickController = this;
                var colliders = new Collider[1];
                Physics.OverlapSphereNonAlloc(startPosition, detectionRadius, colliders, layerMask);
                collider = colliders[0];
                targetSet = true;
                return true;
            }

            pickController = null;
            return false;
        }

        public static implicit operator Vector3(PickController pickController) => pickController.startPosition;

        public static implicit operator Transform(PickController pickController) => pickController.transform;
    }
}