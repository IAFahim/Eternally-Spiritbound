using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul2.Items.Runtime
{
    public interface IItemBase
    {
        public string Guid { get; }
        public string ItemName { get; }
        public string Description { get; }
        public Optional<AssetReferenceGameObject> AssetReferenceGameObject { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }
        public int MaxStack { get; }

        public bool TryPick(GameObject picker, int amount = 1);
        public bool TryUse(GameObject user, int amount = 1);
        public bool TryDrop(GameObject dropper, int amount = 1);
    }
}