using _Root.Scripts.Game.Guid;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class EntityStatsComponent : MonoBehaviour
    {
        public EntityStats entityStats;
        public bool useGlobalStats;
        [SerializeField] EntityStatsScriptable entityStatsScriptable;

        private void Awake()
        {
            var clone = entityStats.Clone();
            if (useGlobalStats && gameObject.TryGetComponent<ITitleGuidReference>(out var titleGuid))
            {
                entityStats = entityStatsScriptable.GetStats(titleGuid.TitleGuid);
            }
            else
            {
                entityStats.vitality.health.current.Value = entityStats.vitality.health.max.Value;
            }
        }
    }
}