using System;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public class SpawnBatchConfig
    {
        public float delaySeconds;
        public AssetReferenceGameObject assetReference;
        public SpawnStrategy spawnStrategy;
        public int max;
        private int _count;

        private AddressableGameObjectPool _pool;

        public void Initialize()
        {
            _pool = new AddressableGameObjectPool(assetReference);
            _count = 0;
        }

        public bool Spawn(Vector3 origin)
        {
            _count += spawnStrategy.Spawn(_pool, origin, max - _count);
            return _count >= max;
        }
    }
}