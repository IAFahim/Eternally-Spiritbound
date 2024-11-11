using System;
using _Root.Scripts.Model.Stats.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    public class EntityStatsComponent : MonoBehaviour
    {
        public int key;
        [SerializeField] private EntityStatParameterScript entityStatsParameterScript;

        [ShowInInspector] [ReadOnly] [NonSerialized]
        public EntityStats entityStats;

        private event Action OnNewEntityStats;
        private event Action OnOldEntityStatsCleanUp;

        private void Start() => SetEntityStats(key);

        public void Register(Action onEntityStatsChange, Action onOldEntityStatsCleanUp)
        {
            OnNewEntityStats += onEntityStatsChange;
            OnOldEntityStatsCleanUp += onOldEntityStatsCleanUp;
            OnNewEntityStats?.Invoke();
        }

        private void OnDisable()
        {
            OnOldEntityStatsCleanUp?.Invoke();
            OnNewEntityStats = null;
            OnOldEntityStatsCleanUp = null;
        }

        [Button]
        public void SetEntityStats(int newKey)
        {
            key = newKey;
            OnOldEntityStatsCleanUp?.Invoke();
            entityStatsParameterScript.TryGetParameter(newKey, out entityStats);
            entityStats.Initialize();
            OnNewEntityStats?.Invoke();
        }
    }
}