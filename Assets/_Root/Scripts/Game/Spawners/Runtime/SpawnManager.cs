using Pancake.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class SpawnManager : MonoBehaviour
    {
        public SpawnerTemplate[] spawnerTemplates;
        private SpawnerTemplate _currentActiveSpawnTemplate;

        [Button]
        public void Spawn(int index)
        {
            if (_currentActiveSpawnTemplate != null) _currentActiveSpawnTemplate.OnStop();
            _currentActiveSpawnTemplate = spawnerTemplates[index];
            _currentActiveSpawnTemplate.OnStart(null);
            App.AddListener(EUpdateMode.Update, OnUpdate);
        }


        private void OnUpdate()
        {
            _currentActiveSpawnTemplate.OnUpdate(transform.position, Time.deltaTime);
        }

        private void OnDisable()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
        }
    }
}