using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Items.Runtime
{
    
    [CreateAssetMenu(menuName = "Scriptable/Asset/Item/Currency", fileName = "CurrencyGameItem")]
    public class CurrencyGameItem : GameItem
    {
        [FormerlySerializedAs("assetOwnAssetGlobalCountLink")] public AssetScriptOwnAssetScriptGlobalCountLink assetScriptOwnAssetScriptGlobalCountLink;

        public override bool OnTryAddToInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount,
            out int addedAmount)
        {
            if (!base.OnTryAddToInventory(assetScriptStorageComponent, amount, out addedAmount)) return false;
            assetScriptOwnAssetScriptGlobalCountLink.Add(
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
            assetScriptOwnAssetScriptGlobalCountLink.Remove(
                assetScriptStorageComponent.GetComponent<AssetScriptReferenceComponent>().assetScriptReference,
                removedAmount
            );
            return true;
        }
    }
}