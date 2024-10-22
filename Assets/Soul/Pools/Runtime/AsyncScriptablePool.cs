using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.Pools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Pools.Runtime
{
    public class AsyncScriptablePool : ScriptableSettings<AsyncScriptablePool>
    {
        [ShowInInspector] private Dictionary<AssetReferenceGameObject, AsyncAddressableGameObjectPool> _pools = new();

        public async UniTask<GameObject> RentAsync(AssetReferenceGameObject assetReferenceGameObject, Transform parent)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new AsyncAddressableGameObjectPool(assetReferenceGameObject);
            }

            return await _pools[assetReferenceGameObject].RentAsync(parent);
        }


        public async UniTask<GameObject> RentAsync(AssetReferenceGameObject assetReferenceGameObject,
            Vector3 position, Quaternion rotation)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new AsyncAddressableGameObjectPool(assetReferenceGameObject);
            }

            return await _pools[assetReferenceGameObject].RentAsync(position, rotation);
        }

        public async UniTask<GameObject> RentAsync(AssetReferenceGameObject assetReferenceGameObject,
            Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new AsyncAddressableGameObjectPool(assetReferenceGameObject);
            }

            return await _pools[assetReferenceGameObject].RentAsync(position, rotation, parent);
        }

        public void Return(AssetReferenceGameObject assetReferenceGameObject, GameObject gameObject)
        {
            _pools[assetReferenceGameObject].Return(gameObject);
        }
        
        public void ClearAll(AssetReferenceGameObject assetReferenceGameObject)
        {
            _pools[assetReferenceGameObject].Clear();
        }

        public void ClearAll()
        {
            foreach (var pool in _pools) pool.Value.Clear();
        }
        
    }
}