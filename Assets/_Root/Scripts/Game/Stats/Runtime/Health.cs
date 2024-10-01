using Alchemy.Inspector;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class Health : MonoBehaviour
    {
        public bool useLocalHealth;
        public bool useLocalMaxHealth;
        public Modifier currentHealth;
        public Modifier maxHealth;


        private void Start()
        {
            var entityStats = GetComponent<EntityStatsComponent>().entityStats;
            var health = entityStats.vitality.health;
            currentHealth = useLocalHealth ? health.max.Clone() : health.current;
            maxHealth = useLocalMaxHealth ? health.max.Clone() : health.max;
        }


        [Button]
        public void TakeDamage(float damage) => currentHealth.Additive -= damage;
    }
}