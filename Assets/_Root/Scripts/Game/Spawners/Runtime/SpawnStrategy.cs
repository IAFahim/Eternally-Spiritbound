using System;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityUtils;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public class SpawnStrategy : ScriptableObject
    {
        public float delaySeconds;
        public AssetReferenceGameObject assetReference;
        public int max;
        public AddressableGameObjectPool Pool;
        
        public int minRadius = 100;
        public int maxRadius = 150;

        public SpawnState Initialize()
        {
            Pool = new AddressableGameObjectPool(assetReference);
            return new SpawnState(this);
        }
        
        public int Spawn(AddressableGameObjectPool pool, Vector3 origin, int limit)
        {
            var position = origin.RandomPointInAnnulus(minRadius, maxRadius);
            pool.Request(position, Quaternion.identity);
            return 1;
        }

        
    }
}