using System;
using Pancake.Pools;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public class SpawnState
    {
        private SpawnStrategy _strategy;
        private int _count;
        private float _delay;
        private float _interval;
        public AddressableGameObjectPool Pool => _strategy.Pool;

        public SpawnState(SpawnStrategy strategy)
        {
            this._strategy = strategy;
        }

        public bool IsAlive(SpawnState spawnState) => spawnState._count <= _strategy.max;

        public bool Update(Vector3 origin, float timeDelta)
        {
            _delay += timeDelta;
            if (_delay <= _strategy.delaySeconds) return true;
            _interval += timeDelta;
            if (_interval <= _strategy.intervalSeconds) return true;
            _count += _strategy.Spawn(_strategy.Pool, origin, _strategy.max - _count);
            _interval = 0;
            return IsAlive(this);
        }
    }
}