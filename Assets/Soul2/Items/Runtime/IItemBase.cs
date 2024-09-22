using UnityEngine;

namespace Soul2.Items.Runtime
{
    public interface IItemBase
    {
        public string ItemName { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public bool Consumable { get; }

        public bool TryPick(GameObject picker, Vector3 position, int amount = 1);
        public bool TryUse(GameObject user, Vector3 position, int amount = 1);
        public bool TryDrop(GameObject dropper, Vector3 position, int amount = 1);
    }
}