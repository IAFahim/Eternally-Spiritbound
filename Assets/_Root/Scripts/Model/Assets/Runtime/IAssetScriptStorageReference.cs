using Soul.Storages.Runtime;

namespace _Root.Scripts.Model.Assets.Runtime
{
    public interface IAssetScriptStorageReference
    {
        IStorageBase<AssetScript, int> AssetScriptStorage { get; }
    }
}