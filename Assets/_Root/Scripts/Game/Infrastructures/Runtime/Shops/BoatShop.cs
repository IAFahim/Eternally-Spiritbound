using System.Collections.Generic;
using _Root.Scripts.Model.Boats.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public class BoatShop : ShopBase
    {
        [SerializeField] private List<BoatVehicleAsset> boatVehicleAssetsInShop;
        [SerializeField] private string[] tabs = { "Boats", "Weapon" };
        [SerializeField] private Transform currentEquipped;

        public List<BoatVehicleAsset> GetItems() => boatVehicleAssetsInShop;

        public override void OnEnter(IInteractorEntryPoint interactorEntryPoint)
        {
            Debug.Log("Shop entered by " + interactorEntryPoint.GameObject.name);
        }

        public override void OnEquipPlacement(Transform itemTransform)
        {
            Debug.Log("Item placed on shop by " + itemTransform.name);
        }

        public override void OnUnEquipPlacement(Transform itemTransform)
        {
            Debug.Log("Item removed from shop by " + itemTransform.name);
        }

        public override void OnLockedEquipPlacement(Transform itemTransform)
        {
            Debug.Log("Item locked on shop by " + itemTransform.name);
        }

        public override void OnUnlockedEquipped()
        {
            Debug.Log("Item unlocked on shop");
        }

        public override void OnExit(IInteractorEntryPoint interactorEntryPoint)
        {
            Debug.Log("Shop exited by " + interactorEntryPoint.GameObject.name);
        }

        public struct BoatShopDto
        {
        }
    }
}