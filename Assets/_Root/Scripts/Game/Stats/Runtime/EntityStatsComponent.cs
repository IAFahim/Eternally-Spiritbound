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

        [ShowInInspector] [NonSerialized] public EntityStats entityStats;

        private event Action OnNewEntityStats;
        private event Action OnOldEntityStatsCleanUp;

        private void Start() => SetEntityStats(level);

        public void RegisterChange(Action onEntityStatsChange, Action onOldEntityStatsCleanUp)
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var size = entityStats.vitality.size;
            var center = entityStats.vitality.Center(transform.position);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, size);
        }
#endif
    }
}