using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Items.Runtime.Storage;
using _Root.Scripts.Game.QuickPickup.Runtime.Handlers;
using Pancake.Pools;
using Soul.QuickPickup.Runtime;
using Soul.Storages.Runtime;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class QuickItemPickupManager : QuickPickupManager<PickupContainer<GameItem>>
    {
        [NonSerialized] public Dictionary<GameItem, AddressableGameObjectPool> Pools;

        [FormerlySerializedAs("detectHandler")]
        public PickupDetectHandlerBase<GameItem> detectHandlerBase;

        [FormerlySerializedAs("activeTweenHandler")]
        public PickupActiveTweenHandlerBase<GameItem> activeTweenHandlerBase;

        [FormerlySerializedAs("pickupHomingHandler")]
        public PickupHomingHandlerBase<GameItem> pickupHomingHandlerBase;

        [FormerlySerializedAs("pickupRecycleHandler")]
        public PickupRecycleHandlerBase<GameItem> pickupRecycleHandlerBase;

        public void Setup(GameItem[] itemBases)
        {
            Pools = new Dictionary<GameItem, AddressableGameObjectPool>();
            base.Enable(new PickupHandlerBase<PickupContainer<GameItem>>[]
            {
                detectHandlerBase,
                activeTweenHandlerBase,
                pickupHomingHandlerBase,
                pickupRecycleHandlerBase
            });
            foreach (var itemBase in itemBases)
            {
                Pools.Add(itemBase, new AddressableGameObjectPool(itemBase));
                itemBase.AddListener(Add);
            }
            detectHandlerBase.HaveSpaceInInventory = HaveSpaceInInventory;
            pickupRecycleHandlerBase.onRecycle = OnRecycle;
        }

        private bool HaveSpaceInInventory(PickupContainer<GameItem> pickupContainer)
        {
            if (pickupContainer.otherTransform.TryGetComponent<IItemStorage>(out var storageReference))
            {
                pickupContainer.StorageReference = storageReference;
                return storageReference.Storage.CanAdd(pickupContainer.element, pickupContainer.amount, out _);
            }

            return false;
        }


        private bool OnRecycle(PickupContainer<GameItem> pickupContainer)
        {
            if (pickupContainer.otherTransform != null)
            {
                pickupContainer.StorageReference.Storage.TryAdd(
                    pickupContainer.element, pickupContainer.amount, out var added
                );
                pickupContainer.amount -= added;
                if (pickupContainer.amount == 0)
                {
                    Pools[pickupContainer.element].Return(pickupContainer.transform.gameObject);
                    return false;
                }
            }


            return true;
        }


        private void Add(ItemDropEvent itemDropEvent)
        {
            var itemBase = itemDropEvent.ItemBase;
            var gameObject = Pools[itemBase].Request(itemDropEvent.Position, Random.rotation);
            var pickupContainer = new PickupContainer<GameItem>(itemBase, gameObject.transform, itemDropEvent.Amount);
            Add(pickupContainer);
        }


        public void Dispose()
        {
            Clear();

            foreach (var (itemBase, pool) in Pools)
            {
                itemBase.RemoveListener(Add);
                pool.Dispose();
            }
        }
    }
}