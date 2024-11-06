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
            if (TryDodge())
            {
                damageDealt = 0;
                return false;
            }

            // Calculate critical damage if applicable
            var afterCritDamage = ApplyChanceMultiplier(damage, critical.chance.Value, critical.damage.Value);

            // Apply armor reduction
            var afterArmor = Mathf.Max(afterCritDamage - defensive.armor.Value, 0);
            damageDealt = afterArmor;

            // Handle shield damage first
            float remainingDamage = damageDealt;
            if (defensive.shield.current.Value > 0)
            {
                float shieldDamage = Mathf.Min(defensive.shield.current.Value, remainingDamage);
                defensive.shield.current.Value -= shieldDamage;
                remainingDamage -= shieldDamage;
            }

            // Apply remaining damage to health
            if (remainingDamage > 0)
            {
                vitality.health.current.Value -= remainingDamage;
            }

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

        public void Heal(float amount)
        {
            vitality.health.current.Value =
                Mathf.Min(vitality.health.current.Value + amount, vitality.health.max.Value);
        }

        public void RestoreShield(float amount)
        {
            defensive.shield.current.Value =
                Mathf.Min(defensive.shield.current.Value + amount, defensive.shield.max.Value);
        }


        public bool TryDodge()
        {
            return Random.value < defensive.dodgeChance.Value;
        }
        
        public void AddExperience(int amount)
        {
            float bonusExp = amount * (1 + progression.experienceRate.Value);
            progression.experience.Value += Mathf.RoundToInt(bonusExp);
        }
        
        public bool CanAttack(float currentTime)
        {
            return currentTime >= offensive.cooldown.Value;
        }
        
        public float GetAttackAccuracy()
        {
            // Returns a value between 0 and 1, where 1 is perfect accuracy
            return Mathf.Clamp01(offensive.accuracy.Value);
        }
    }
}