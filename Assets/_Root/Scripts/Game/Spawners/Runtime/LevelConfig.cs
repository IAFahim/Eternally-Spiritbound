using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class LevelConfig : MonoBehaviour
    {
        public MainObjectProviderScriptable mainObjectProvider;
        public SpawnBatchConfig[] spawnBatchConfigs;
        public List<SpawnBatchConfig> activeSpawnBatchConfigs;

        public void Start()
        {
            foreach (var spawnBatchConfig in spawnBatchConfigs)
            {
                spawnBatchConfig.Initialize();
                activeSpawnBatchConfigs.Add(spawnBatchConfig);
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
                    activeSpawnBatchConfigs.Remove(spawnBatchConfig);
                }
            }
        }
    }
}