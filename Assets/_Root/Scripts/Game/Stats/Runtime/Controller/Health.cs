using _Root.Scripts.Game.Stats.Runtime.Model;
using Sirenix.OdinInspector;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    public class Health
    {
        private readonly LimitStat<Modifier> _health;
        private readonly Modifier _armor;
        private readonly LimitStat<Modifier> _shield;
        private readonly CriticalStats<Modifier> _criticalStats;

        public Health(LimitStat<Modifier> health, Modifier armor, LimitStat<Modifier> shield,
            CriticalStats<Modifier> criticalStats)
        {
            _health = health;
            _armor = armor;
            _shield = shield;
            _criticalStats = criticalStats;
            _health.current.SetValueWithoutNotify(health.max.Value);
        }

        public float HealthPercentage => _health.current / _health.max.Value;

        [Button]
        private void DamageTest(float damage)
        {
            TryKill(damage, out var damageTaken);
        }

        public bool TryKill(float damage, out float damageTaken)
        {
            var afterCritDamage =
                ApplyChanceMultiplier(damage, _criticalStats.chance.Value, _criticalStats.damage.Value);
            var afterArmor = afterCritDamage - _armor.Value;
            damageTaken = Mathf.Max(afterArmor, 0);
            var afterShield = damageTaken - _shield.current.Value;
            _shield.current.Value = Mathf.Max(_shield.current.Value - damageTaken, 0);
            _health.current.Value -= Mathf.Max(damageTaken - _shield.current.Value, 0);
            Debug.Log(
                $"Damage: {damage}, Crit: {afterCritDamage}, Armor: {afterArmor}, Shield: {afterShield}, Health: {_health.current.Value}");
            return _health.current.Value <= 0;
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