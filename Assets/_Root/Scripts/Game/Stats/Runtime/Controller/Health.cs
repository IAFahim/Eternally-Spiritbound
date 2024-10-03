using _Root.Scripts.Game.Stats.Runtime.Model;
using Alchemy.Inspector;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    public class Health
    {
        private readonly LimitStat<Modifier> health;
        private readonly Modifier armor;
        private readonly LimitStat<Modifier> shield;
        private readonly CriticalStats<Modifier> criticalStats;

        public Health(LimitStat<Modifier> health, Modifier armor, LimitStat<Modifier> shield, CriticalStats<Modifier> criticalStats)
        {
            this.health = health;
            this.armor = armor;
            this.shield = shield;
            this.criticalStats = criticalStats;
        }

        public float HealthPercentage => health.current / health.max.Value;

        [Button]
        private void DamageTest(float damage)
        {
            Damage(damage, out var damageTaken);
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