using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Items.Runtime
{
    
    [CreateAssetMenu(menuName = "Scriptable/Asset/Item/Currency", fileName = "CurrencyGameItem")]
    public class CurrencyGameItem : GameItem
    {
        public AssetOwnAssetGlobalCountLink assetOwnAssetGlobalCountLink;

        public override bool OnTryAddToInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount,
            out int addedAmount)
        {
            if (!base.OnTryAddToInventory(assetScriptStorageComponent, amount, out addedAmount)) return false;
            assetOwnAssetGlobalCountLink.Add(
                assetScriptStorageComponent.GetComponent<AssetScriptReferenceComponent>().assetScriptReference,
                addedAmount
            );
            return true;
        }

        public override bool OnTryRemovedFromInventory(AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int removedAmount)
        {
            if (!base.OnTryRemovedFromInventory(assetScriptStorageComponent, amount, out removedAmount)) return false;
            assetOwnAssetGlobalCountLink.Remove(
                assetScriptStorageComponent.GetComponent<AssetScriptReferenceComponent>().assetScriptReference,
                removedAmount
            );
            return true;
        }
    }
}