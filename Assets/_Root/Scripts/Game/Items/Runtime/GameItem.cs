using System;
using _Root.Scripts.Game.Interactables;
using Pancake;
using Soul.Items.Runtime;
using Soul.Storages.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Items.Runtime
{
    [Serializable]
    [CreateAssetMenu(fileName = "Coin", menuName = "Scriptable/Items/New")]
    public class GameItem : Event<ItemDropEvent>, IItemBase<GameObject>, IPickupStrategy
    {
        [Guid] public string guid;
        public AssetReferenceGameObject assetReferenceGameObject;

        [SerializeField] private string itemName;
        [SerializeField, TextArea(3, 10)] public string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack;
        [SerializeField] private bool consumable;

        public PickUpDropStrategy pickUpDropStrategy;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool IsStackable => maxStack > 1;
        
        public bool AutoPickup => pickUpDropStrategy.autoPickup;
        public float PickupRange => pickUpDropStrategy.range;

        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }


        public bool CanPick<TComponent>(GameObject picker, Vector3 position, int amount,
            out TComponent pickerComponent) where TComponent : IStorageBase<string, int>
        {
            return picker.TryGetComponent(out pickerComponent) &&
                   pickerComponent.CanAdd(this, amount, out _);
        }

        public bool TryPick(GameObject picker, Vector3 position, int amount)
        {
            return CanPick(picker, position, amount, out IStorageBase<string, int> storage) &&
                   storage.TryAdd(this, amount, out int added) && added == amount;
        }

        public bool CanUse<TComponent>(GameObject user, Vector3 position, int amount,
            out TComponent userComponent) where TComponent : IStorageBase<string, int>
        {
            return user.TryGetComponent(out userComponent) && userComponent.HasEnough(this, amount);
        }


        public bool TryUse(GameObject user, Vector3 position, int amount)
        {
            if (CanUse(user, position, amount, out IStorageBase<string, int> storage))
            {
                storage.TryRemove(this.name, amount, out _);
                return true;
            }

            return false;
        }

        public bool CanSpawn(Vector3 position, int amount)
        {
            return true;
        }

        public bool CanDrop<TComponent>(GameObject dropper, Vector3 position, int amount,
            out TComponent dropperComponent) where TComponent : IStorageBase<string, int>
        {
            return dropper.TryGetComponent(out dropperComponent) && dropperComponent.HasEnough(this, amount);
        }


        public bool TrySpawn(Vector3 position, int amount)
        {
            Trigger(new ItemDropEvent(this, position, amount));
            return true;
        }

        public bool TryDrop(GameObject dropper, Vector3 position, int amount)
        {
            if (dropper.TryGetComponent(out IStorageBase<string, int> storage))
            {
                bool dropHasInStock = storage.TryRemove(this.name, amount, out int removed) && removed == amount;
                if (dropHasInStock) Trigger(new ItemDropEvent(this, position, amount));
                return dropHasInStock;
            }

            return false;
        }

        public static implicit operator Sprite(GameItem itemBase) => itemBase.Icon;
        public static implicit operator string(GameItem itemBase) => itemBase.guid;

        public static implicit operator AssetReferenceGameObject(GameItem itemBase) =>
            itemBase.assetReferenceGameObject;

        
    }
}