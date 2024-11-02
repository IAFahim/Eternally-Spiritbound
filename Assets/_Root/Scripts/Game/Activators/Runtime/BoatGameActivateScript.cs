using _Root.Scripts.Game.Ai.Runtime.Targets;
using _Root.Scripts.Game.Spawners.Runtime;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    [CreateAssetMenu(fileName = "BoatGame Activator", menuName = "Scriptable/Activators/BoatGame")]
    public class BoatGameActivateScript : ActivatorScript
    {
        public TargetStrategy targetStrategy;
        public SpawnerTemplate[] spawnerTemplates;
        private SpawnerTemplate _currentActiveSpawnTemplate;
        
        public override void Activate(Transform activatorInvoker)
        {
            Spawn(0);
        }
        
        public void Spawn(int index)
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
        
        public override void Deactivate(Transform activatorInvoker)
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
        }

        public override void CleanUp()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
            targetStrategy.Stop();
        }
    }
}