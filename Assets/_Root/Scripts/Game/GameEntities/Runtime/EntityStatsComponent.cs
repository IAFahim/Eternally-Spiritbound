using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    public class EntityStatsComponent : MonoBehaviour
    {
        public EntityStatParameterScript entityStatsParameterScript;
        public EntityStats entityStats;

        public void Awake()
        {
            entityStats = entityStatsParameterScript.value;
            entityStats.Initialize();
        }
    }
}