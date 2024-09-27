using System;
using Pancake;
using Soul.Items.Runtime;
using Soul.Storages.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Items.Runtime
{
    [Serializable]
    public abstract class ItemBase : Event<ItemDropEvent>, IItemBase<GameObject>
    {
        [Guid] public string guid;
        public AssetReferenceGameObject assetReferenceGameObject;

        [SerializeField] private string itemName;
        [SerializeField, TextArea(3, 10)] public string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack;
        [SerializeField] private bool consumable;
        
        public float pickupRadius = 5f;
        public bool autoPickup = true;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool IsStackable => maxStack > 1;

        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }

        public abstract bool CanPick<TComponent>(GameObject picker, Vector3 position, int amount,
            out TComponent pickerComponent) where TComponent : IStorageBase<string, int>;

        public abstract bool TryPick(GameObject picker, Vector3 position, int amount);

        public abstract bool CanUse<TComponent>(GameObject user, Vector3 position, int amount,
            out TComponent userComponent) where TComponent : IStorageBase<string, int>;

        public abstract bool TryUse(GameObject user, Vector3 position, int amount);
        public abstract bool CanSpawn(Vector3 position, int amount);

        public abstract bool CanDrop<TComponent>(GameObject dropper, Vector3 position, int amount,
            out TComponent dropperComponent) where TComponent : IStorageBase<string, int>;

        public abstract bool TrySpawn(Vector3 position, int amount);
        public abstract bool TryDrop(GameObject dropper, Vector3 position, int amount);

        public static implicit operator Sprite(ItemBase itemBase) => itemBase.Icon;
        public static implicit operator string(ItemBase itemBase) => itemBase.guid;

        public static implicit operator AssetReferenceGameObject(ItemBase itemBase) =>
            itemBase.assetReferenceGameObject;
    }
}