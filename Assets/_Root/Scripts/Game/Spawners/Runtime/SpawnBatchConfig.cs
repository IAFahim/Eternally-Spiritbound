using System;
using LitMotion;
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
        private float _timePassedSceneLast;
        private int _count;

        private AddressableGameObjectPool _pool;

        public void Initialize()
        {
            _pool = new AddressableGameObjectPool(assetReference);
            _count = 0;
            _timePassedSceneLast = 0;
        }

        public bool Update(Vector3 origin, float timeDelta)
        {
            _timePassedSceneLast += timeDelta;
            if (_timePassedSceneLast >= delaySeconds)
            {
                _count += spawnStrategy.Spawn(_pool, origin, max - _count);
                _timePassedSceneLast = 0;
                return _count < max;    
            }

            return true;
        }
    }
}