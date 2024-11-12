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
        [Guid] public string guid;
        [FormerlySerializedAs("assetReferenceGameObject")] public AssetReferenceGameObject assetReference;
        [TextArea(3, 10)] public string description;
        [CanBeNull] public Sprite icon;
    }
}