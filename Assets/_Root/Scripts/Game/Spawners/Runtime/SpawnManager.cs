using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class SpawnManager : MonoBehaviour
    {
        public SpawnerTemplate[] spawnerTemplates;
        private SpawnerTemplate _currentActiveSpawnTemplate;

        private void Start()
        {
            Spawn(0);
        }

        public void Spawn(int index)
        {
            if (_currentActiveSpawnTemplate != null) _currentActiveSpawnTemplate.OnStop();
            _currentActiveSpawnTemplate = spawnerTemplates[index];
            _currentActiveSpawnTemplate.OnStart(null);
            App.AddListener(EUpdateMode.Update, OnUpdate);
        }


        private void OnUpdate()
        {
            _currentActiveSpawnTemplate.OnUpdate(Time.deltaTime);
        }

        private void OnDisable()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
        }
    }
}