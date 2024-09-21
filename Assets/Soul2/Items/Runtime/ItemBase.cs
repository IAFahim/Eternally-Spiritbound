using System;
using System.Runtime.Serialization;
using Pancake;
using Soul2.Storages.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul2.Items.Runtime
{
    [Serializable]
    public abstract class ItemBase : ScriptableObject, IItemBase
    {
        [SerializeField, Guid] private string guid;
        [SerializeField] private string itemName;
        [SerializeField, TextArea(3, 10)] public string description;
        [SerializeField] private Optional<AssetReferenceGameObject> assetReferenceGameObject;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack = 1;
        [SerializeField] private bool consumable;

        public string Guid => guid;
        public string ItemName => itemName;
        public string Description => description;
        public Optional<AssetReferenceGameObject> AssetReferenceGameObject => assetReferenceGameObject;
        public Sprite Icon => icon;
        public bool Consumable => consumable;
        public bool IsStackable => maxStack > 1;
        public int MaxStack => maxStack;

        public abstract bool TryPick(GameObject picker, int amount = 1);

        public abstract bool TryUse(GameObject user, int amount = 1);

        public abstract bool TryDrop(GameObject dropper, int amount = 1);
    }
}