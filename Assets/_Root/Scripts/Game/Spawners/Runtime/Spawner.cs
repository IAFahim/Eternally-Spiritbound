using System;
using _Root.Scripts.Game.ObjectModifers.Runtime;
using _Root.Scripts.Game.Placements.Runtime;
using Sisus.Init.Reflection;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public class Spawner
    {
        [SerializeField] private AssetReferenceGameObject assetReference;
        [SerializeField] protected ScriptablePool pool;
        [FormerlySerializedAs("spawnStrategy")] [SerializeField] private PlacementStrategy placementStrategy;
        [SerializeField] private GameObjectModifer gameObjectModifer;
        [SerializeField] private float startDelay;
        [SerializeField] private float interval;

        [SerializeField] private int spawned;
        [SerializeField] private int count = 100;
        private float _passedTime;
        private float _intervalTimer;

        public void Initialize()
        {
            spawned = 0;
            _passedTime = 0;
            _intervalTimer = 0;
        }

        public bool IsAlive() => spawned < count;

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
            gameObject.AddComponent<SpawnHandle>(this);
            gameObjectModifer.Modify(gameObject);
            spawned += placementStrategy.Place(gameObject.transform, count - spawned);
            return IsAlive();
        }

        public void Despawn(GameObject gameObject)
        {
            gameObjectModifer.UnModify(gameObject);
            pool.Return(assetReference, gameObject);
        }
    }
}