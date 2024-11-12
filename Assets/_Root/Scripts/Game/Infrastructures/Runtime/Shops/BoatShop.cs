using System.Collections.Generic;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Boats.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public class BoatShop : ShopBase
    {
        [SerializeField] private List<BoatVehicleAsset> boatVehicleAssetsInShop;
        [SerializeField] private Transform currentEquipped;
        
        public List<BoatVehicleAsset> GetItems() => boatVehicleAssetsInShop;

        public override void OnEnter(IInteractorEntryPoint interactorEntryPoint)
        {
            Debug.Log("Shop entered by " + interactorEntryPoint.GameObject.name);
        }

        public override void OnUnlockedSelected(AssetScript assetScript)
        {
            Debug.Log("Item placed on shop by " + assetScript.Value);
        }
        
        public override void OnLockedItemSelected(AssetScript assetScript)
        {
            Debug.Log("Item locked on shop by " + assetScript.Value);
        }

        public override void OnDeSelected(AssetScript assetScript)
        {
            Debug.Log("Item removed from shop by " + assetScript.Value);
        }

        public override void OnUnlocked(AssetScript assetScript)
        {
            Debug.Log("Item unlocked on shop");
        }

        public override void OnExit(IInteractorEntryPoint interactorEntryPoint)
        {
            Debug.Log("Shop exited by " + interactorEntryPoint.GameObject.name);
        }
    }
}