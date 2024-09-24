using System;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Items.Runtime.Storage;
using _Root.Scripts.Game.QuickPickup.Runtime.Handlers;
using Pancake.Pools;
using Soul.QuickPickup.Runtime;
using Soul.Storages.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class QuickItemPickupManager : QuickPickupManager<ItemBase>
    {
        [NonSerialized] public AddressableGameObjectPool pool;
        public PickupDetectHandler<ItemBase> detectHandler;
        public PickupActiveTweenHandler<ItemBase> activeTweenHandler;
        public PickupHomingHandler<ItemBase> pickupHomingHandler;
        public PickupRecycleHandler<ItemBase> pickupRecycleHandler;

        public void Enable(ItemBase element)
        {
            base.Enable(element, new PickupHandler<ItemBase>[]
            {
                detectHandler,
                activeTweenHandler,
                pickupHomingHandler,
                pickupRecycleHandler
            });
            element.AddListener(Add);
            pickupRecycleHandler.onRecycle += OnRecycle;
            pool = new AddressableGameObjectPool(element);
        }

        private bool OnRecycle(PickupContainer<ItemBase> pickupContainer)
        {
            if (pickupContainer.otherTransform.TryGetComponent<IIntStorageReference<ItemBase>>(out var storageReference))
            {
                storageReference.Storage.TryAdd(pickupContainer.element, pickupContainer.amount, out var added);
                pickupContainer.amount -= added;
                if (pickupContainer.amount == 0)
                {
                    pool.Return(pickupContainer.transform.gameObject);
                    return false;
                }
            }

            return true;
        }


        private void Add(ItemDropEvent itemDropEvent)
        {
            Add(itemDropEvent.Position, itemDropEvent.Amount);
        }

        public void Add(Vector3 position, int amount)
        {
            var gameObject = pool.Request(position, Random.rotation);
            Add(gameObject, amount);
        }


        public void Disable()
        {
            Clear();
            pool.Dispose();
            elementReference.RemoveListener(Add);
        }
    }
}