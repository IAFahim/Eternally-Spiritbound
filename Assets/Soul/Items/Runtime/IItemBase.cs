using UnityEngine;

namespace Soul.Items.Runtime
{
    public interface IItemBase<in T>
    {
        public string ItemName { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }
        void Initialize(T user, int amount);
        bool TryPick(T picker, Vector3 position, int amount);
        bool TryUse(T user, Vector3 position, int amount);
    }
}