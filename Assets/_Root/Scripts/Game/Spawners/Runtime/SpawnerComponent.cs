using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    public class SpawnerComponent : MonoBehaviour
    {
        public SpawnerTemplate[] spawnerTemplates;
        public int selectedIndex = 0;

        private SpawnerTemplate _currentActiveSpawnTemplate;

        private void OnEnable()
        {
            Spawn(selectedIndex);
        }

        [Button]
        private void Spawn(int index)
        {
            if (index < 0 || index >= spawnerTemplates.Length) return;
            _currentActiveSpawnTemplate = spawnerTemplates[index];
            _currentActiveSpawnTemplate.OnStart(OnComplete);
        }

        private void OnComplete()
        {
            Debug.Log("OnComplete");
        }

        public void Update()
        {
            _currentActiveSpawnTemplate.OnUpdate(Time.deltaTime);
        }
    }
}