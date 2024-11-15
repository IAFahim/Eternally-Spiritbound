using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    [RequireComponent(typeof(SingleShopAndBoatConnection))]
    public class BoatShop : ShopBase
    {
        [SerializeField] private AssetScriptDataBase assetScriptDataBase;
        [SerializeField] private SingleShopAndBoatConnection singleShopAndBoatConnection;

        [FormerlySerializedAs("assetScriptPriceLink")] [SerializeField]
        private AssetPriceLink assetPriceLink;

        [FormerlySerializedAs("assetOwnAssetCountGlobalLink")]
        [FormerlySerializedAs("assetScriptOwnAssetCountGlobalLink")]
        [FormerlySerializedAs("assetScriptsOwnAssetCountGlobalLink")]
        [SerializeField]
        private AssetOwnAssetGlobalCountLink assetOwnAssetGlobalCountLink;

        private AssetScript _currentAssetScript;

        protected override void OnEnable()
        {
            base.OnEnable();
            equippedItemGuid = PlayerPrefs.GetString(name, equippedItemGuid);
            SpawnEquippedBoat(equippedItemGuid);
        }

        public override void OnEnter(IInteractorEntryPoint interactorEntryPoint)
        {
        }

        public override void OnDeSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript)
        {
            if (assetScript.Guid == _currentAssetScript.Guid) return;
            singleShopAndBoatConnection.DespawnBoat(_currentAssetScript);
        }

        public override void OnUnlockedSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript)
        {
            SpawnBoat(assetScript, true);
        }

        public override void OnLockedItemSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript)
        {
            SpawnBoat(assetScript, false);
        }

        public override bool OnTryBuyButtonClick(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category, AssetScript assetScript, out string message)
        {
            message = string.Empty;
            return true;
        }

        public override bool HasEnough(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            AssetScript item,
            out AssetPrice assetPrice)
        {
            if (assetPriceLink.TryGetValue(item, out assetPrice))
            {
                if (assetOwnAssetGlobalCountLink.TryGetValue(item, out var count))
                {
                    var hasEnough = count >= assetPrice.price;
                    if (!hasEnough) Debug.Log("Not enough money");
                    return hasEnough;
                }
            }

            return false;
        }

        public override void OnExit(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsMain) SpawnEquippedBoat(equippedItemGuid);
        }

        private void SpawnEquippedBoat(string guid)
        {
            if (assetScriptDataBase.TryGetValue(guid, out var assetScript)) SpawnBoat(assetScript, false);
        }

        private void SpawnBoat(AssetScript assetScript, bool save)
        {
            if (_currentAssetScript == assetScript) return;
            if (_currentAssetScript != null) singleShopAndBoatConnection.DespawnBoat(_currentAssetScript);
            if (save)
            {
                equippedItemGuid = assetScript.Guid;
                PlayerPrefs.SetString(name, equippedItemGuid);
            }

            _currentAssetScript = assetScript;
            singleShopAndBoatConnection.SpawnBoat(assetScript).Forget();
        }
    }
}