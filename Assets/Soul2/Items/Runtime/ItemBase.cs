using System;
using UnityEngine;

namespace Soul2.Items.Runtime
{
    [Serializable]
    public abstract class ItemBase : ScriptableObject, IItemBase
    {
        [SerializeField] private string itemName;
        [SerializeField, TextArea(3, 10)] public string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack = 1;
        [SerializeField] private bool consumable;
        public abstract string Guid { get; }
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public bool Consumable => consumable;

        public bool IsStackable
        {
            get => maxStack > 1;
            set => maxStack = value ? 1 : 0;
        }

        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }

        public abstract bool TryPick(GameObject picker, Vector3 position, int amount = 1);
        public abstract bool TryUse(GameObject user, Vector3 position, int amount = 1);
        public abstract bool TryDrop(GameObject dropper, Vector3 position, int amount = 1);

        public static implicit operator Sprite(ItemBase item) => item.Icon;
    }
}