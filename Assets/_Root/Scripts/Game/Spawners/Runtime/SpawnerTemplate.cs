using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Spawner/Template/New", fileName = "Spawner Template")]
    public class SpawnerTemplate : ScriptableObject
    {
        public Spawner[] spawners;
        private event Action OnComplete;

        private List<Spawner> _activeSpawners;

        protected virtual bool IsComplete => _activeSpawners.Count == 0;

        public void OnStart(Action onComplete)
        {
            OnComplete = onComplete;
            _activeSpawners = new List<Spawner>();
            foreach (var spawner in spawners)
            {
                spawner.Initialize();
                _activeSpawners.Add(spawner);
            }
        }

        public void OnUpdate(Vector3 position, float deltaTime)
        {
            if (IsComplete)
            {
                OnStop();
                return;
            }

            ManageSpawners(position, deltaTime);
        }

        public void OnStop()
        {
            OnComplete?.Invoke();
        }

        protected virtual void ManageSpawners(Vector3 position, float deltaTime)
        {
            for (var index = _activeSpawners.Count - 1; index >= 0; index--)
            {
                var spawner = _activeSpawners[index];
                if (spawner.KeepSpawning(position, deltaTime)) return;
                _activeSpawners.RemoveAt(index);
            }
        }
    }
}