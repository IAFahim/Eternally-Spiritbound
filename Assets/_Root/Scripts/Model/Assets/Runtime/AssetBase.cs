using JetBrains.Annotations;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Model.Assets.Runtime
{
    public class AssetBase : StringConstant
    {
        [Guid] public string guid;
        public AssetReferenceGameObject assetReferenceGameObject;
        [TextArea(3, 10)] public string description;
        [CanBeNull] public Sprite icon;
    }
}