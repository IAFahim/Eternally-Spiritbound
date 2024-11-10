using _Root.Scripts.Game.Ai.Runtime.Targets;
using _Root.Scripts.Game.Spawners.Runtime;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public class BoatGameActivate : MonoBehaviour
    {
        public TargetStrategy targetStrategy;
        public SpawnerTemplate[] spawnerTemplates;
        private SpawnerTemplate _currentActiveSpawnTemplate;

        public void OnEnable()
        {
            Spawn(0);
        }

        private void Spawn(int index)
        {
            if (_currentActiveSpawnTemplate != null) _currentActiveSpawnTemplate.OnStop();
            _currentActiveSpawnTemplate = spawnerTemplates[index];
            _currentActiveSpawnTemplate.OnStart(OnComplete);
            App.AddListener(EUpdateMode.Update, OnUpdate);
        }

        private void OnComplete()
        {
            Debug.Log("OnComplete");
        }

        private void OnUpdate()
        {
            _currentActiveSpawnTemplate.OnUpdate(Time.deltaTime);
        }

        private void OnDisable()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
            targetStrategy.Stop();
        }
    }
}