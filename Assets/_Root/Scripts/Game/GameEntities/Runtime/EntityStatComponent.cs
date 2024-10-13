using _Root.Scripts.Game.Stats.Runtime.Controller;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    public class EntityStatComponent : MonoBehaviour, IEntityStatsReference
    {
        public EntityStats entityStats;
        public EntityStats EntityStats => entityStats;
        public void Awake()
        {
            entityStats.Initialize();
        }
    }
}