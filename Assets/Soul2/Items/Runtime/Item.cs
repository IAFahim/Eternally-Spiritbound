using System;
using Pancake;
using Soul2.Storages.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul2.Items.Runtime
{
    [Serializable]
    public abstract class Item : ScriptableObject, IItem
    {
        [Guid] string guid;
        public string itemName;
        [TextArea(3, 10)] public string description;
        public AssetReferenceGameObject assetReferenceGameObject;
        public Sprite icon;
        public int maxStack;
        public bool consumable;

        public string Guid => guid;
        public string ItemName => itemName;
        public string Description => description;
        public AssetReferenceGameObject AssetReferenceGameObject => assetReferenceGameObject;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool IsStackable => maxStack > 1;
        public int MaxStack => maxStack;
        
        public virtual bool TryPick(GameObject picker, IStorage<IItem> storage, int amount = 1)
        {
            return storage.TryAdd(this, amount, out var added);
        }

        public virtual bool TryUse(GameObject user, IStorage<IItem> storage, int amount = 1)
        {
            return Consumable && storage.TryRemove(this, amount, out int removed);
        }

        public virtual bool TryDrop(GameObject dropper, IStorage<IItem> storage, int amount = 1)
        {
            return storage.TryRemove(this, amount, out int removed);
        }
    }
}