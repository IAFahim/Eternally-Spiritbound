using _Root.Scripts.Model.Assets.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    [RequireComponent(typeof(SingleShopAndBoatConnection))]
    public class BoatShop : ShopBase
    {
        [SerializeField] private string guidDefaultBoat;
        [SerializeField] private AssetScriptDataBase assetScriptDataBase;
        [SerializeField] private SingleShopAndBoatConnection singleShopAndBoatConnection;

        private AssetScript _currentAssetScript;

        protected override void OnEnable()
        {
            base.OnEnable();
            guidDefaultBoat = PlayerPrefs.GetString(name, guidDefaultBoat);
            if (assetScriptDataBase.TryGetValue(guidDefaultBoat, out var assetScript)) SpawnBoat(assetScript, false);
        }

        public override void OnDeSelected(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript)
        {
            base.OnDeSelected(playerAssetScriptComponent, category, assetScript);
            if (assetScript.guid == _currentAssetScript.guid) return;
            singleShopAndBoatConnection.DespawnBoat(_currentAssetScript);
        }

        public override void OnUnlockedSelected(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript)
        {
            base.OnUnlockedSelected(playerAssetScriptComponent, category, assetScript);
            SpawnBoat(assetScript);
        }

        public override void OnLockedItemSelected(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript)
        {
            base.OnLockedItemSelected(playerAssetScriptComponent, category, assetScript);
            SpawnBoat(assetScript);
        }

        private void SpawnBoat(AssetScript assetScript, bool save = true)
        {
            guidDefaultBoat = assetScript.guid;
            PlayerPrefs.SetString(name, guidDefaultBoat);
            _currentAssetScript = assetScript;
            singleShopAndBoatConnection.SpawnBoat(assetScript).Forget();
        }
    }
}