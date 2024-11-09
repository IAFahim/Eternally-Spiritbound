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

        public override void OnEquipPlacement(Transform equipped)
        {
            Debug.Log($"Equipped {equipped.name}");
        }

        public override void OnUnEquipPlacement(Transform equipped)
        {
            Debug.Log($"UnEquipped {equipped.name}");
        }

        public override void OnLockedEquipPlacement(Transform equipped)
        {
            Debug.Log($"Locked {equipped.name}");
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