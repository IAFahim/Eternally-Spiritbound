using System;
using UnityEngine;

namespace Soul.QuickPickup.Runtime
{
    [Serializable]
    public struct PickupContainerBase<T>
    {
        public T element;
        public Transform transform;
        public Vector3 startPosition;
        public int amount;


        public PickupContainerBase(T element, Transform transform , int staringAmount)
        {
            this.element = element;
            this.transform = transform;
            startPosition = transform.position;
            amount = staringAmount;
        }

        public static implicit operator Vector3(PickupContainerBase<T> pickupContainer) => pickupContainer.startPosition;
    }
}