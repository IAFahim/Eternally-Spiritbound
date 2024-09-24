using System;
using UnityEngine;

namespace Soul.QuickPickup.Runtime
{
    [Serializable]
    public class PickupContainer<T>
    {
        public T element;
        public Transform transform;
        public Vector3 startPosition;
        public int amount;
        public Transform otherTransform;
        

        public PickupContainer(Transform transform, T element, int staringAmount)
        {
            this.element = element;
            this.transform = transform;
            startPosition = transform.position;
            amount = staringAmount;
        }

        public static implicit operator Vector3(PickupContainer<T> pickupContainer) => pickupContainer.startPosition;
    }
}