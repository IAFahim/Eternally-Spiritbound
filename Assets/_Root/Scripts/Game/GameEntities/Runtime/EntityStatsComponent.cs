using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-999)]
    public class EntityStatsComponent : MonoBehaviour
    {
        public EntityStatParameterScript entityStatsParameterScript;
        public EntityStats entityStats;

        private void OnEnable()
        {
            entityStats = entityStatsParameterScript.New();
            entityStats.Initialize();
        }
    }
}