using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using Cysharp.Threading.Tasks;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    [RequireComponent(typeof(SingleShopAndBoatConnection))]
    public class BoatShop : ShopBase
    {
        [SerializeField] protected AssetCategory[] assetCategories;
        [SerializeField] private AssetScriptDataBase assetScriptDataBase;
        [SerializeField] private SingleShopAndBoatConnection singleShopAndBoatConnection;

        [SerializeField] private AssetScriptPriceLink assetScriptPriceLink;

        [SerializeField] private AssetScriptOwnAssetScriptGlobalCountLink assetScriptOwnAssetScriptGlobalCountLink;
        private AssetScript _currentAssetScript;
        private View _currentView;

        protected override void OnEnable()
        {
            base.OnEnable();
            equippedItemGuid = PlayerPrefs.GetString(name, equippedItemGuid);
            SpawnEquippedBoat(equippedItemGuid);
        }

        public override AssetCategory[] GetAssetCategories() => assetCategories;

        public override void OnEnter(IInteractorEntryPoint interactorEntryPoint)
        {
        }

        public override void OnDeSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            int categoryIndex,
            AssetScript assetScript)
        {
            if (assetScript.Guid == _currentAssetScript.Guid) return;
            singleShopAndBoatConnection.DespawnBoat(_currentAssetScript);
        }

        public override void OnUnlockedSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            int categoryIndex,
            AssetScript assetScript)
        {
            SpawnBoat(assetScript, categoryIndex, true, true).Forget();
        }

        public override void OnLockedItemSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            int categoryIndex,
            AssetScript assetScript)
        {
            SpawnBoat(assetScript, categoryIndex, false, true).Forget();
        }

        public override bool OnTryBuyButtonClick(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            int categoryIndex, AssetScript assetScript, out string message)
        {
            message = string.Empty;
            return true;
        }

        public override bool HasEnough(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            AssetScript item,
            out AssetPrice assetPrice)
        {
            if (assetScriptPriceLink.TryGetValue(item, out assetPrice))
            {
                if (assetScriptOwnAssetScriptGlobalCountLink.TryGetValue(item, out var count))
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
            if (interactorEntryPoint.IsMain)
            {
                SpawnEquippedBoat(equippedItemGuid);
                _currentView.Return();
            }
        }

        private void SpawnEquippedBoat(string guid)
        {
            if (assetScriptDataBase.TryGetValue(guid, out var assetScript))
                SpawnBoat(assetScript, 0, false, false).Forget();
        }

        private async UniTaskVoid SpawnBoat(AssetScript assetScript, int categoryIndex, bool save, bool spawnView)
        {
            if (_currentAssetScript != assetScript)
            {
                if (_currentAssetScript != null) singleShopAndBoatConnection.DespawnBoat(_currentAssetScript);
                if (save)
                {
                    equippedItemGuid = assetScript.Guid;
                    PlayerPrefs.SetString(name, equippedItemGuid);
                }

                _currentAssetScript = assetScript;
                currentPreviewGameObject = await singleShopAndBoatConnection.SpawnBoat(assetScript);
            }
            
            if (spawnView)
            {
                var assetCategory = assetCategories[categoryIndex];
                if (_currentView) _currentView.Return();
                _currentView = assetCategory.view;
                if (assetCategory.gameObjectView) ((IGameObjectView)assetCategory.view).Init(currentPreviewGameObject);
            }
        }
    }
}