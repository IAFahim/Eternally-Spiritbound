using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class LevelConfig : MonoBehaviour
    {
        public MainObjectProviderScriptable mainObjectProvider;
        public SpawnStrategy[] spawnBatchConfigs;
        public List<SpawnState> activeSpawnBatchConfigs;
        public List<SpawnState> inactiveSpawnBatchConfigs;

        public void Start()
        {
            foreach (var spawnBatchConfig in spawnBatchConfigs)
            {
                activeSpawnBatchConfigs.Add(spawnBatchConfig.Initialize());
            }
        }

        public void Update()
        {
            var position = mainObjectProvider.mainGameObjectInstance.transform.position;
            var deltaTime = Time.deltaTime;
            for (var index = activeSpawnBatchConfigs.Count - 1; index >= 0; index--)
            {
                var spawnBatchConfig = activeSpawnBatchConfigs[index];
                if (!spawnBatchConfig.Update(position, deltaTime))
                {
                    inactiveSpawnBatchConfigs.Add(spawnBatchConfig);
                    activeSpawnBatchConfigs.Remove(spawnBatchConfig);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var spawnBatchConfig in activeSpawnBatchConfigs) spawnBatchConfig.Pool.Dispose();
            foreach (var spawnBatchConfig in inactiveSpawnBatchConfigs) spawnBatchConfig.Pool.Dispose();
        }
    }
}