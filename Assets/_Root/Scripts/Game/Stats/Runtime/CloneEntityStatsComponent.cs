using _Root.Scripts.Game.Guid;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Pancake.Pools;
using Sisus.Init;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class CloneEntityStatsComponent : MonoBehaviour, IEntityStats, IPoolCallbackReceiver
    {
        public EntityStatsScriptable entityStatsScriptable;
        public EntityStatsBase<Modifier> EntityStats { get; private set; }

        private void Awake()
        {
            var entityStats = (EntityStatsBase<Modifier>)entityStatsScriptable
                .GetStats(gameObject.GetComponent<ITitleGuidReference>().TitleGuid).Clone();
            EntityStats = entityStats;

            gameObject
                .AddComponent<Health, LimitStat<Modifier>, Modifier, LimitStat<Modifier>, CriticalStats<Modifier>>(
                    entityStats.vitality.health, entityStats.defensive.armor, entityStats.defensive.shield,
                    entityStats.critical);
        }

        public void OnRequest()
        {
        }

        public void OnReturn()
        {
        }
    }
}