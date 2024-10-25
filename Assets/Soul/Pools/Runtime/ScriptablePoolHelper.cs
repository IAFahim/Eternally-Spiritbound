using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Pools.Runtime
{
    public static class ScriptablePoolHelper
    {
        public static GameObject Request(this AssetReferenceGameObject asset)
        {
            return ScriptablePool.Instance.Request(asset);
        }

        public static GameObject Request(this AssetReferenceGameObject asset, Transform parent,
            bool instantiateInWorldSpace)
        {
            return ScriptablePool.Instance.Request(asset, parent, instantiateInWorldSpace);
        }

        public static GameObject Request(this AssetReferenceGameObject asset, Vector3 position, Quaternion rotation)
        {
            return ScriptablePool.Instance.Request(asset, position, rotation);
        }

        public static GameObject Request(this AssetReferenceGameObject asset, Vector3 position, Quaternion rotation,
            Transform parent)
        {
            return ScriptablePool.Instance.Request(asset, position, rotation, parent);
        }

        public static GameObject Request(this AssetReferenceGameObject asset, Transform parent)
        {
            return ScriptablePool.Instance.Request(asset, parent);
        }
    }
}