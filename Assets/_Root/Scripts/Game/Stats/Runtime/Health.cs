using Alchemy.Inspector;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public class Health : MonoBehaviour
    {
        private RegenStat<Modifier> health;
        private Modifier armor;
        private RegenStat<Modifier> shield;
        private CriticalStats<Modifier> criticalStats;

        private void Awake()
        {
            var entityStats = GetComponent<EntityStatsComponent>().entityStats;
            health = entityStats.vitality.health;
            armor = entityStats.defensive.armor;
            shield = entityStats.defensive.shield;
            criticalStats = entityStats.critical;
        }


        public float HealthPercentage => health.current / health.max.Value;

        [Button]
        private void DamageTest(float damage)
        {
            Damage(damage, out var damageTaken);
            Debug.Log($"Damage taken: {damageTaken}");
        }

        public void Damage(float damage, out float damageTaken)
        {
            var afterCritDamage = ApplyChanceMultiplier(damage, criticalStats.chance.Value, criticalStats.damage.Value);
            var afterArmor = afterCritDamage - armor.Value;
            damageTaken = Mathf.Max(afterArmor, 0);
            var afterShield = damageTaken - shield.current.Value;
            shield.current.Value = Mathf.Max(shield.current.Value - damageTaken, 0);
            health.current.Value -= Mathf.Max(damageTaken - shield.current.Value, 0);
        }

        /// <summary>
        /// Applies a chance-based rate to the current Value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chance">The probability of applying the bonus rate. If >= 1, it's treated as a guaranteed rate.</param>
        /// <param name="rate">The rate to apply if the chance check succeeds.</param>
        /// <returns>The result of Value multiplied by the determined rate.</returns>
        public float ApplyChanceMultiplier(float value, float chance, float rate)
        {
            float chanceMultiplier;
            if (chance >= 1f)
                chanceMultiplier = chance;
            else if (Random.value < chance)
                chanceMultiplier = rate + 1;
            else
                chanceMultiplier = 1f;
            return value * chanceMultiplier;
        }
    }
}