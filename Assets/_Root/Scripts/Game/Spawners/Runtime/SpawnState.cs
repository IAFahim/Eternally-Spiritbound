using System;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public class SpawnState
    {
        public float timePassedSceneLast;
        public int count;
        public SpawnStrategy strategy;
        public AddressableGameObjectPool Pool => strategy.Pool;

        public SpawnState(SpawnStrategy strategy)
        {
            this.strategy = strategy;
        }
        
        public bool IsAlive(SpawnState spawnState) => spawnState.count < strategy.max;

        public bool Update(Vector3 origin, float timeDelta)
        {
            timePassedSceneLast += timeDelta;
            if (!(timePassedSceneLast >= strategy.delaySeconds)) return true;
            count += strategy.Spawn(strategy.Pool, origin, strategy.max - count);
            timePassedSceneLast = 0;
            return IsAlive(this);
        }
        
    }
}