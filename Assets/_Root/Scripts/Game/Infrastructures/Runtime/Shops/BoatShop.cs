using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Relationships.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    [RequireComponent(typeof(SingleShopAndBoatConnection))]
    public class BoatShop : ShopBase
    {
        [FormerlySerializedAs("guidDefaultBoat")] [SerializeField]
        private string equippedBoatGuid;

        [SerializeField] private AssetScriptDataBase assetScriptDataBase;
        [SerializeField] private SingleShopAndBoatConnection singleShopAndBoatConnection;
        [SerializeField] private AssetPriceLink assetPriceLink;
        private AssetScript _currentAssetScript;

        protected override void OnEnable()
        {
            base.OnEnable();
            equippedBoatGuid = PlayerPrefs.GetString(name, equippedBoatGuid);
            SpawnEquippedBoat(equippedBoatGuid);
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

        public override void OnExit(IInteractorEntryPoint interactorEntryPoint)
        {
            SpawnEquippedBoat(equippedBoatGuid);
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
                equippedBoatGuid = assetScript.Guid;
                PlayerPrefs.SetString(name, equippedBoatGuid);
            }

            _currentAssetScript = assetScript;
            singleShopAndBoatConnection.SpawnBoat(assetScript).Forget();
        }
    }
}