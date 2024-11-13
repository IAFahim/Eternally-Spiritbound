using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Assets.Runtime
{
    public class AssetScriptStorageComponent : MonoBehaviour, IAssetScriptStorageReference
    {
        public AssetScriptStorage assetScriptStorage;
        public IStorageBase<AssetScript, int> AssetScriptStorage => assetScriptStorage;

        private void OnEnable()
        {
            assetScriptStorage.InitializeStorage();
            foreach (var (key, value) in assetScriptStorage) key.OnAddedToInventory(this, value);
        }
    }
}