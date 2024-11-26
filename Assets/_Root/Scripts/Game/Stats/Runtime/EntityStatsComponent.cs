using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [DisallowMultipleComponent]
    public class EntityStatsComponent : MonoBehaviour
    {
        public int level;
        [SerializeField] private EntityStatParameterScript entityStatsParameterScript;

        [ShowInInspector] [ReadOnly] [NonSerialized]
        public EntityStats entityStats;

        private event Action OnNewEntityStats;
        private event Action OnOldEntityStatsCleanUp;

        private void Start() => SetEntityStats(level);

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
            level = newKey;
            OnOldEntityStatsCleanUp?.Invoke();
            entityStatsParameterScript.TryGetParameter(newKey, out entityStats);
            entityStats.Initialize();
            OnNewEntityStats?.Invoke();
        }
    }
}