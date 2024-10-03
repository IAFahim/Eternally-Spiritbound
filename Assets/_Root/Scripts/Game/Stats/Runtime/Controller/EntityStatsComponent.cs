using _Root.Scripts.Game.Guid;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    public class EntityStatsComponent : MonoBehaviour, IEntityStats, IPoolCallbackReceiver
    {
        public EntityStatsScriptable entityStatsScriptable;
        public bool cloneStats = true;
        private EntityStats _entityStats;
        private Health _health;
        public EntityStats EntityStats => _entityStats;


        private void Awake()
        {
            var stats = entityStatsScriptable.GetStats(gameObject.GetComponent<ITitleGuidReference>().TitleGuid);
            _entityStats = cloneStats ? (EntityStats)stats.Clone() : stats;
            _health = new Health(
                EntityStats.vitality.health,
                EntityStats.defensive.armor,
                EntityStats.defensive.shield,
                EntityStats.critical
            );
        }

        public void OnRequest()
        {
        }

        public void OnReturn()
        {
        }
    }
}