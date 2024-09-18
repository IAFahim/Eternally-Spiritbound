using Soul2.Storages.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul2.Items.Runtime
{
    public interface IItem
    {
        public string Guid { get; }
        public string ItemName { get; }
        public string Description { get; }
        public AssetReferenceGameObject AssetReferenceGameObject { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }
        public int MaxStack { get; }

        public bool TryPick(GameObject picker, IStorage<IItem> storage, int amount = 1);
        public bool TryUse(GameObject user, IStorage<IItem> storage, int amount = 1);
        public bool TryDrop(GameObject dropper, IStorage<IItem> storage, int amount = 1);
    }
}