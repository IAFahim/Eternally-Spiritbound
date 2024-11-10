using System;
using _Root.Scripts.Game.ObjectModifers.Runtime;
using _Root.Scripts.Game.Placements.Runtime;
using Sirenix.OdinInspector;
using Sisus.Init.Reflection;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public class Spawner
    {
        [SerializeField] private AssetReferenceGameObject assetReference;
        [SerializeField] private ScriptablePool pool;
        [SerializeField] private PlacementStrategy placementStrategy;
        [SerializeField] private GameObjectModifer gameObjectModifer;
        [SerializeField] private float startDelay;
        [SerializeField] private float interval;
        [SerializeField] private int count = 10;

        [ShowInInspector] [ReadOnly] private int _spawned;
        private float _passedTime;
        private float _intervalTimer;

        public void Initialize()
        {
            _spawned = 0;
            _passedTime = 0;
            _intervalTimer = 0;
        }

        public bool IsAlive() => _spawned < count;

        private bool CanSpawnThisFrame(float timeDelta)
        {
            _passedTime += timeDelta;
            if (_passedTime < startDelay) return false; // Wait for the start delay

            _intervalTimer += timeDelta;
            if (_intervalTimer >= interval)
            {
                _intervalTimer -= interval;
                return true;
            }

            return false;
        }

        public bool KeepSpawning(float deltaTime)
        {
            if (!CanSpawnThisFrame(deltaTime)) return true;
            GameObject gameObject = pool.Request(assetReference);
            gameObject.AddComponent<SpawnHandle>(gameObjectModifer);
            _spawned += placementStrategy.Place(gameObject.transform, count - _spawned);
            return IsAlive();
        }
    }
}