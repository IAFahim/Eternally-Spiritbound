using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public class Shop : ShopBase
    {
        public Transform currentEquipped;

        public override void OnEnter(Transform other)
        {
            Debug.Log($"Entered {other.name}");
        }

        public override void OnEquipPlacement(Transform itemTransform)
        {
            Debug.Log($"Equipped {itemTransform.name}");
        }

        public override void OnUnEquipPlacement(Transform itemTransform)
        {
            Debug.Log($"UnEquipped {itemTransform.name}");
        }

        public override void OnLockedEquipPlacement(Transform itemTransform)
        {
            Debug.Log($"Locked {itemTransform.name}");
        }

        public override void OnUnlockedEquipped()
        {
            Debug.Log("Unlocked");
        }

        public override void OnExit(Transform other)
        {
            Debug.Log($"Exited {other.name}");
        }
    }
}