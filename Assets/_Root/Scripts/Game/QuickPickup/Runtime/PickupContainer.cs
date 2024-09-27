using System;
using _Root.Scripts.Game.Items.Runtime.Storage;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class PickupContainer<T> : PickupContainerBase<T>
    {
        public IItemStorage StorageReference;
        public PickupContainer(T element, Transform transform, int staringAmount) : base(element, transform, staringAmount)
        {
        }
    }
}