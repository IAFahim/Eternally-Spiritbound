using System;
using UnityEngine;

namespace Soul.Items.Runtime
{
    public interface IItemBase<in T>
    {
        public string Description { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }
        void OnPick(T picker);
        void OnUse(T user);
        void OnDrop(T user, Vector3 position, int amount);
    }
}