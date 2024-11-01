using System;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Sirenix.OdinInspector;
using Soul.Modifiers.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    [Serializable]
    public struct EntityStats
    {
        public VitalityStats<Modifier> vitality;
        public DefensiveStats<Modifier> defensive;
        public MovementStats<Modifier> movement;
        public ProgressionStats<Modifier> progression;
        public CriticalStats<Modifier> critical;
        public AmmoStats<Modifier> ammo;
        public OffensiveStats<Modifier> offensive;

        public void Initialize()
        {
            vitality.health.current.SetValueWithoutNotify(vitality.health.max.Value);
        }

        public float HealthPercentage => vitality.health.current / vitality.health.max.Value;

        [Button]
        private void DamageTest(float damage)
        {
            TryKill(damage, out var damageTaken);
        }

        public bool TryKill(float damage, out float damageDealt)
        {
            var afterCritDamage = ApplyChanceMultiplier(damage, critical.chance.Value, critical.damage.Value);
            var afterArmor = afterCritDamage - defensive.armor.Value;
            damageDealt = Mathf.Max(afterArmor, 0);
            var afterShield = damageDealt - defensive.shield.current.Value;
            defensive.shield.current.Value = Mathf.Max(defensive.shield.current.Value - damageDealt, 0);
            vitality.health.current.Value -= Mathf.Max(damageDealt - defensive.shield.current.Value, 0);
            return vitality.health.current.Value <= 0;
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