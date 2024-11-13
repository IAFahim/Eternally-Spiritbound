using JetBrains.Annotations;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Asset/AssetScript", fileName = "AssetScript")]
    public class AssetScript : StringConstant
    {
        [Guid] [SerializeField] protected string guid;
        [SerializeField] protected AssetReferenceGameObject assetReference;
        [TextArea(3, 10)] [SerializeField] protected string description;
        [CanBeNull] [SerializeField] protected Sprite icon;

        public string Guid => guid;
        public AssetReferenceGameObject AssetReference => assetReference;
        public string Description => description;
        public Sprite Icon => icon;


        public virtual void OnAddedToInventory(AssetScriptStorageComponent assetScriptStorageComponent, int amount)
        {
        }
    }
}