using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public abstract class ShopBase : MonoBehaviour
    {
        public abstract void OnEnter(Transform other);
        public abstract void OnEquipPlacement(Transform equipped);
        public abstract void OnUnEquipPlacement(Transform equipped);
        public abstract void OnLockedEquipPlacement(Transform equipped);
        public abstract void OnUnlockedEquipped();
        public abstract void OnExit(Transform other);
    }
}