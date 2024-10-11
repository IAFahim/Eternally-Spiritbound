using System;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public abstract class SpawnStrategy : ScriptableObject
    {
        public AssetReferenceGameObject assetReference;
        public float delaySeconds;
        public float intervalSeconds;
        public int max;
        
        public AddressableGameObjectPool Pool;
        
        public virtual SpawnState Initialize()
        {
            Pool = new AddressableGameObjectPool(assetReference);
            return new SpawnState(this);
        }
        
        public abstract int Spawn(AddressableGameObjectPool pool, Vector3 origin, int limit);
    }
}