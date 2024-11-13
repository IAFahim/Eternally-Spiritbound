using System;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class PickupContainer<T> : PickupContainerBase<T>
    {
        public IAssetScriptStorageReference StorageReferenceReference;
        public PickupContainer(T element, Transform transform, int staringAmount) : base(element, transform, staringAmount)
        {
        }
    }
}