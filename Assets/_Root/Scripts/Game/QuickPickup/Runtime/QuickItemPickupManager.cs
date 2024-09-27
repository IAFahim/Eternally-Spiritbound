using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.QuickPickup.Runtime.Handlers;
using Pancake.Pools;
using Soul.QuickPickup.Runtime;
using Soul.Storages.Runtime;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class QuickItemPickupManager : QuickPickupManager<ItemBase>
    {
        [NonSerialized] public Dictionary<ItemBase, AddressableGameObjectPool> Pools;
        public PickupDetectHandler<ItemBase> detectHandler;
        public PickupActiveTweenHandler<ItemBase> activeTweenHandler;
        public PickupHomingHandler<ItemBase> pickupHomingHandler;
        public PickupRecycleHandler<ItemBase> pickupRecycleHandler;

        public void Setup(ItemBase[] itemBases)
        {
            Pools = new Dictionary<ItemBase, AddressableGameObjectPool>();
            base.Enable(new PickupHandler<ItemBase>[]
            {
                detectHandler,
                activeTweenHandler,
                pickupHomingHandler,
                pickupRecycleHandler
            });
            foreach (var itemBase in itemBases)
            {
                Pools.Add(itemBase, new AddressableGameObjectPool(itemBase));
                itemBase.AddListener(Add);
                pickupRecycleHandler.onRecycle += OnRecycle;
            }
        }


        private bool OnRecycle(PickupContainer<ItemBase> pickupContainer)
        {
            if (pickupContainer.otherTransform
                .TryGetComponent<IIntStorageReference<ItemBase>>(out var storageReference))
            {
                storageReference.Storage.TryAdd(pickupContainer.element, pickupContainer.amount, out var added);
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
            Add(itemDropEvent.ItemBase, gameObject.transform, itemDropEvent.Amount);
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