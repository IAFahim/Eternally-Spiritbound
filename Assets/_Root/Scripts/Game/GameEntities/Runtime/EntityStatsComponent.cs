using System;
using _Root.Scripts.Model.Stats.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-999)]
    public class EntityStatsComponent : MonoBehaviour
    {
        public EntityStatParameterScript entityStatsParameterScript;

        [ShowInInspector] [ReadOnly] [NonSerialized]
        public EntityStats entityStats;

        private event Action OnNewEntityStats;
        private event Action OnOldEntityStatsCleanUp;


        private void OnEnable()
        {
            SetEntityStats(0);
        }

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
        public void SetEntityStats(int key)
        {
            OnOldEntityStatsCleanUp?.Invoke();
            entityStatsParameterScript.TryGetParameter(key, out entityStats);
            entityStats.Initialize();
            OnNewEntityStats?.Invoke();
        }
    }
}