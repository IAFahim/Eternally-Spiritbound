using _Root.Scripts.Game.Guid;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class EntityStatsComponent : MonoBehaviour, IEntityStats, IPoolCallbackReceiver
    {
        public EntityStatsScriptable entityStatsScriptable;
        public bool cloneStats=true; 
        public EntityStats EntityStats { get; private set; }
        public Health Health { get; private set; }
        
        
        private void Awake()
        {
            var entityStats = entityStatsScriptable.GetStats(gameObject.GetComponent<ITitleGuidReference>().TitleGuid);
            EntityStats = cloneStats ? (EntityStats)entityStats.Clone() : entityStats;
            Health = new Health(EntityStats);
        }

        public void OnRequest()
        {
        }

        public void OnReturn()
        {
        }
    }
}