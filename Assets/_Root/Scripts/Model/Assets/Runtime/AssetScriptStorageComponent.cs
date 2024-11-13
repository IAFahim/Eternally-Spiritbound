using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Assets.Runtime
{
    public class AssetScriptStorageComponent : MonoBehaviour, IAssetScriptStorageReference
    {
        [SerializeField] private AssetScriptStorage assetScriptStorage;
        public IStorageBase<AssetScript, int> AssetScriptStorage => assetScriptStorage;

        private void OnEnable()
        {
            assetScriptStorage.InitializeStorage();
            foreach (var (key, value) in assetScriptStorage) key.PresentInInventory(this, value);
        }
    }
}