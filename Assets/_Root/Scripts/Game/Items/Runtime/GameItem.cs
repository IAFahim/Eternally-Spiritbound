using Pancake;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Items.Runtime
{
    public abstract class GameItem : ItemBase
    {
        [Guid] public string guid;
        public AssetReferenceGameObject assetReferenceGameObject;
        public static implicit operator AssetReferenceGameObject(GameItem gameItem) =>
            gameItem.assetReferenceGameObject;
        public static implicit operator string(GameItem gameItem) => gameItem.guid;
    }
}