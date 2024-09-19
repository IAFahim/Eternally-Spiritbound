using Pancake;
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
        public Optional<AssetReferenceGameObject> AssetReferenceGameObject { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }
        public int MaxStack { get; }

        public bool TryPick(GameObject picker, IStorageBase<IItem> storageBase, int amount = 1);
        public bool TryUse(GameObject user, IStorageBase<IItem> storageBase, int amount = 1);
        public bool TryDrop(GameObject dropper, IStorageBase<IItem> storageBase, int amount = 1);
    }
}