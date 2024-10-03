using _Root.Scripts.Game.Guid;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    [CreateAssetMenu(fileName = "EntityStats", menuName = "Scriptable/Stats/EntityStats")]
    public class EntityStatsScriptable : ScriptableObject
    {
        [SerializeField] private UnityDictionary<TitleGuid, EntityStats> allStats;
        public EntityStats GetStats(TitleGuid titleGuid) => allStats[titleGuid];
    }
}