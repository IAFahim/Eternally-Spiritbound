using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public abstract class ShopBase : MonoBehaviour
    {
        public abstract void OnEnter(Transform other);
        public abstract void OnEquipPlacement(Transform itemTransform);
        public abstract void OnUnEquipPlacement(Transform itemTransform);
        public abstract void OnLockedEquipPlacement(Transform itemTransform);
        public abstract void OnUnlockedEquipped();
        public abstract void OnExit(Transform other);
    }
}