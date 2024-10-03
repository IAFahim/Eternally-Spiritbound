using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Items.Runtime.Storage;
using _Root.Scripts.Game.QuickPickup.Runtime.Handlers;
using Pancake.Pools;
using Soul.QuickPickup.Runtime;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class QuickItemPickupManager : QuickPickupManager<PickupContainer<GameItem>>
    {
        [NonSerialized] public Dictionary<GameItem, AddressableGameObjectPool> Pools;

        public PickupDetectHandlerBase<GameItem> detectHandlerBase;

        public PickupActiveTweenHandlerBase<GameItem> activeTweenHandlerBase;

        public PickupHomingHandlerBase<GameItem> pickupHomingHandlerBase;

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
            if (pickupContainer.otherTransform.TryGetComponent<IGameItemStorageReference>(out var storageReference))
            {
                pickupContainer.StorageReferenceReference = storageReference;
                return storageReference.GameItemStorage.CanAdd(pickupContainer.element, pickupContainer.amount, out _);
            }

            return false;
        }


        private bool OnRecycle(PickupContainer<GameItem> pickupContainer)
        {
            if (pickupContainer.otherTransform != null)
            {
                pickupContainer.StorageReferenceReference.GameItemStorage.TryAdd(
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