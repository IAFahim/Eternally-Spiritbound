using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;
using Pancake;
using Pancake.Pools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Soul.Pools.Runtime
{
    public class ScriptablePool : ScriptableSettings<ScriptablePool>
    {
        [ShowInInspector] private Dictionary<AssetReferenceGameObject, AddressableGameObjectPool> _pools = new();
        bool _cleared;
        
        public GameObject Request(AssetReferenceGameObject assetReferenceGameObject)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new (assetReferenceGameObject);
            }

            return _pools[assetReferenceGameObject].Request();
        }

        public GameObject Request(AssetReferenceGameObject assetReferenceGameObject, Transform parent)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new (assetReferenceGameObject);
            }

            return _pools[assetReferenceGameObject].Request(parent);
        }

        public GameObject Request(AssetReferenceGameObject assetReferenceGameObject, Transform parent,
            bool instantiateInWorldSpace)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new (assetReferenceGameObject);
            }

            var gameObject = _pools[assetReferenceGameObject].Request();
            gameObject.transform.SetParent(parent, instantiateInWorldSpace);
            return gameObject;
        }

        public GameObject Request(AssetReferenceGameObject assetReferenceGameObject, Vector3 position,
            Quaternion rotation)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new (assetReferenceGameObject);
            }

            return _pools[assetReferenceGameObject].Request(position, rotation);
        }

        public GameObject Request(AssetReferenceGameObject assetReferenceGameObject, Vector3 position,
            Quaternion rotation, Transform parent)
        {
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new (assetReferenceGameObject);
            }

            return _pools[assetReferenceGameObject].Request(position, rotation, parent);
        }

        public void Return(AssetReferenceGameObject assetReferenceGameObject, GameObject gameObject)
        {
            if (_cleared) return;
            if (!_pools.ContainsKey(assetReferenceGameObject))
            {
                _pools[assetReferenceGameObject] = new (assetReferenceGameObject);
            }

            _pools[assetReferenceGameObject].Return(gameObject);
        }

        public void ClearAll(AssetReferenceGameObject assetReferenceGameObject)
        {
            _pools[assetReferenceGameObject].Clear();
        }

        public void ClearAll()
        {
            _cleared = true;
            foreach (var pool in _pools) pool.Value.Clear();
        }

        public void Dispose() => ClearAll();
    }
}