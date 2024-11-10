using System.Collections.Generic;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Pools.Runtime
{
    public static class SharedAssetPool
    {
        private static readonly Dictionary<AssetReferenceGameObject, AddressableGameObjectPool> Pools = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            ClearAll();
            Pools.Clear();
        }

        public static GameObject Request(AssetReferenceGameObject original)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            return GetOrCreatePool(original).Request();
        }

        public static GameObject Request(AssetReferenceGameObject original, Transform parent)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            return GetOrCreatePool(original).Request(parent);
        }

        public static GameObject Request(AssetReferenceGameObject original, Transform parent,
            bool instantiateInWorldSpace)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            var gameObject = GetOrCreatePool(original).Request();
            gameObject.transform.SetParent(parent, instantiateInWorldSpace);
            return gameObject;
        }

        public static GameObject Request(AssetReferenceGameObject original, Vector3 position, Quaternion rotation)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            return GetOrCreatePool(original).Request(position, rotation);
        }

        public static GameObject Request(AssetReferenceGameObject original, Vector3 position, Quaternion rotation,
            Transform parent)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            return GetOrCreatePool(original).Request(position, rotation, parent);
        }

        public static void Return(AssetReferenceGameObject original, GameObject gameObject)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            if (gameObject == null) throw new System.ArgumentNullException(nameof(gameObject));
            GetOrCreatePool(original).Return(gameObject);
        }

        public static void ClearAll(AssetReferenceGameObject original)
        {
            if (original == null) throw new System.ArgumentNullException(nameof(original));
            GetOrCreatePool(original).Clear();
        }

        public static void ClearAll()
        {
            foreach (var pool in Pools.Values) pool.Clear();
        }

        private static AddressableGameObjectPool GetOrCreatePool(AssetReferenceGameObject original)
        {
            if (Pools.TryGetValue(original, out var pool)) return pool;
            pool = new AddressableGameObjectPool(original);
            Pools.Add(original, pool);

            return pool;
        }
    }
}