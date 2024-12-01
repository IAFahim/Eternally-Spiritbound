using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Pools.Runtime
{
    public static class SharedAssetPoolInactive
    {
        private static readonly Dictionary<AssetReferenceGameObject, AsyncInactiveAddressablePool> Pools = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            DisposeAll();
        }

        public static async UniTask<GameObject> RequestAsync(AssetReferenceGameObject original)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            return await GetOrCreatePool(original).RequestAsync();
        }

        public static async UniTask<GameObject> RequestAsync(AssetReferenceGameObject original, Transform parent,
            bool worldPositionStays = false)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            return await GetOrCreatePool(original).RequestAsync(parent, worldPositionStays);
        }

        public static async UniTask<GameObject> RequestAsync(AssetReferenceGameObject original, Vector3 position,
            Quaternion rotation)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            return await GetOrCreatePool(original).RequestAsync(position, rotation);
        }

        public static async UniTask<GameObject> RequestAsync(AssetReferenceGameObject original, Vector3 position,
            Quaternion rotation, Transform parent)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            return await GetOrCreatePool(original).RequestAsync(position, rotation, parent);
        }

        public static async UniTask<TComponent> RequestAsync<TComponent>(AssetReferenceGameObject original)
            where TComponent : Component
        {
            var temp = await RequestAsync(original);
            return temp.GetComponent<TComponent>();
        }

        public static async UniTask<TComponent> RequestAsync<TComponent>(AssetReferenceGameObject original,
            Transform parent,
            bool worldPositionStays = false)
            where TComponent : Component
        {
            var temp = await RequestAsync(original, parent, worldPositionStays);
            return temp.GetComponent<TComponent>();
        }

        public static async UniTask<TComponent> RequestAsync<TComponent>(AssetReferenceGameObject original,
            Vector3 position,
            Quaternion rotation) where TComponent : Component
        {
            var temp = await RequestAsync(original, position, rotation);
            return temp.GetComponent<TComponent>();
        }

        public static async UniTask<TComponent> RequestAsync<TComponent>(AssetReferenceGameObject original,
            Vector3 position,
            Quaternion rotation, Transform parent)
            where TComponent : Component
        {
            var temp = await RequestAsync(original, position, rotation, parent);
            return temp.GetComponent<TComponent>();
        }

        public static void Return(AssetReferenceGameObject original, GameObject gameObject)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            GetOrCreatePool(original).Return(gameObject);
        }

        public static void Dispose(AssetReferenceGameObject original)
        {
            if (Pools.TryGetValue(original, out var pool))
            {
                pool.Dispose();
                Pools.Remove(original);
            }
        }

        private static void DisposeAll()
        {
            foreach (var pool in Pools.Values) pool.Dispose();
            Pools.Clear();
        }

        private static AsyncInactiveAddressablePool GetOrCreatePool(AssetReferenceGameObject original)
        {
            if (Pools.TryGetValue(original, out var pool)) return pool;
            pool = new AsyncInactiveAddressablePool(original);
            Pools.Add(original, pool);
            return pool;
        }
    }
}