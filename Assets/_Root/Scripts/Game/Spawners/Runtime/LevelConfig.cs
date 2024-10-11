using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class LevelConfig : MonoBehaviour
    {
        public SpawnStrategy[] spawnBatchConfigs;
        
        private List<SpawnState> _activeSpawnBatchConfigs;
        private List<SpawnState> _inactiveSpawnBatchConfigs;

        public void Start()
        {
            _activeSpawnBatchConfigs = new List<SpawnState>();
            _inactiveSpawnBatchConfigs = new List<SpawnState>();
            foreach (var spawnBatchConfig in spawnBatchConfigs)
            {
                _activeSpawnBatchConfigs.Add(spawnBatchConfig.Initialize());
            }
        }

        public void Update()
        {
            var position = transform.position;
            var deltaTime = Time.deltaTime;
            for (var index = _activeSpawnBatchConfigs.Count - 1; index >= 0; index--)
            {
                var spawnBatchConfig = _activeSpawnBatchConfigs[index];
                if (!spawnBatchConfig.Update(position, deltaTime))
                {
                    _inactiveSpawnBatchConfigs.Add(spawnBatchConfig);
                    _activeSpawnBatchConfigs.Remove(spawnBatchConfig);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var spawnBatchConfig in _activeSpawnBatchConfigs) spawnBatchConfig.Pool.Dispose();
            foreach (var spawnBatchConfig in _inactiveSpawnBatchConfigs) spawnBatchConfig.Pool.Dispose();
        }
    }
}