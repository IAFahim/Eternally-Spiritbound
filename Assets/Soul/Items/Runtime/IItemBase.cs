using UnityEngine;

namespace Soul.Items.Runtime
{
    public interface IItemBase<in T>
    {
        public string ItemName { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }

        bool TryPick(T picker, Vector3 position, int amount);
        bool TryUse(T user, Vector3 position, int amount);
        bool TrySpawn(Vector3 position, int amount);
        bool TryDrop(T dropper, Vector3 position, int amount);
    }
}