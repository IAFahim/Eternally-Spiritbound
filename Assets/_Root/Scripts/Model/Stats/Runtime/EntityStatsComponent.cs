using System;
using Pancake;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [DisallowMultipleComponent]
    public class EntityStatsComponent : MonoBehaviour, IInitializable<int>
    {
        public int level;
        [SerializeField] private EntityStatParameterScript entityStatsParameterScript;
        [ShowInInspector] [NonSerialized] public EntityStats entityStats;
        public Optional<Rigidbody> rigidbody;
        private bool isInitialized;

        private event Action OnNewEntityStats;
        private event Action OnOldEntityStatsCleanUp;

        private void Start()
        {
            Init(level);
        }
        
        public void Init(int newKey)
        {
            if (isInitialized) OnOldEntityStatsCleanUp?.Invoke();
            level = newKey;
            isInitialized = true;
            entityStatsParameterScript.TryGetParameter(newKey, out entityStats);
            entityStats.Initialize();
            OnNewEntityStats?.Invoke();
        }

        public void RegisterChange(Action onEntityStatsChange, Action onOldEntityStatsCleanUp)
        {
            OnNewEntityStats += onEntityStatsChange;
            OnOldEntityStatsCleanUp += onOldEntityStatsCleanUp;
            if (isInitialized) onEntityStatsChange?.Invoke();
        }

        public void UnregisterChange(Action onEntityStatsChange, Action onOldEntityStatsCleanUp)
        {
            OnNewEntityStats -= onEntityStatsChange;
            OnOldEntityStatsCleanUp -= onOldEntityStatsCleanUp;
        }

        private void OnDisable()
        {
            OnOldEntityStatsCleanUp?.Invoke();
            OnNewEntityStats = null;
            OnOldEntityStatsCleanUp = null;
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

        private void Reset()
        {
            var rigidbodyComponent = GetComponent<Rigidbody>();
            rigidbody = new Optional<Rigidbody>(rigidbodyComponent, rigidbodyComponent);
        }
    }
}