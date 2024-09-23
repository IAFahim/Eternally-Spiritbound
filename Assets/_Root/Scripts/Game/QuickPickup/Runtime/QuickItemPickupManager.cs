using System;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.QuickPickup.Runtime.Handlers;
using _Root.Scripts.Game.Storages;
using Pancake.Pools;
using Soul2.QuickPickup.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.QuickPickup.Runtime
{
    [Serializable]
    public class QuickItemPickupManager : QuickPickupManager<GameItem>
    {
        [NonSerialized] public AddressableGameObjectPool pool;
        public PickupDetectHandler<GameItem> detectHandler;
        public PickupActiveTweenHandler<GameItem> activeTweenHandler;
        public PickupHomingHandler<GameItem> pickupHomingHandler;
        public PickupRecycleHandler<GameItem> pickupRecycleHandler;

        public void Enable(GameItem element)
        {
            base.Enable(element, new PickupHandler<GameItem>[]
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

        private bool OnRecycle(PickupContainer<GameItem> pickupContainer)
        {
            if (pickupContainer.otherTransform.TryGetComponent<IStringStorageReference>(out var storageReference))
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


        private void Add(GameItemDropEvent gameItemDropEvent)
        {
            Add(gameItemDropEvent.Position, gameItemDropEvent.Amount);
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