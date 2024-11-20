using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Items.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Asset/Item/Currency", fileName = "CurrencyGameItem")]
    public class CurrencyGameItem : GameItem
    {
        [FormerlySerializedAs("assetOwnAssetGlobalCountLink")]
        public AssetScriptOwnAssetScriptGlobalCountLink assetScriptOwnAssetScriptGlobalCountLink;

        public override bool OnTryAddToInventory(
            AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int addedAmount,
            out int afterAddAmount)
        {
            if (!base.OnTryAddToInventory(assetScriptStorageComponent, amount, out addedAmount, out afterAddAmount))
                return false;
            assetScriptOwnAssetScriptGlobalCountLink.Add(
                assetScriptStorageComponent.GetComponent<AssetScriptReferenceComponent>().assetScriptReference,
                addedAmount
            );
            return true;
        }

        public override bool OnTryRemovedFromInventory(AssetScriptStorageComponent assetScriptStorageComponent,
            int amount,
            out int removedAmount,
            out int afterRemoveAmount
        )
        {
            if (!base.OnTryRemovedFromInventory(assetScriptStorageComponent, amount, out removedAmount,
                    out afterRemoveAmount))
                return false;
            assetScriptOwnAssetScriptGlobalCountLink.Remove(
                assetScriptStorageComponent.GetComponent<AssetScriptReferenceComponent>().assetScriptReference,
                removedAmount
            );
            return true;
        }
    }
}