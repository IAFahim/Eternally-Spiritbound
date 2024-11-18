using System;
using Sirenix.OdinInspector;
using Soul.Modifiers.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    [Searchable]
    public struct EntityStats
    {
        public VitalityStats vitality;
        public DefensiveStats defensive;
        public MovementStats movement;
        public ProgressionStats progression;
        public CriticalStats critical;
        public AmmoStats ammo;
        public OffensiveStats offensive;

        public void Initialize()
        {
            vitality.health.current.SetValueWithoutNotify(vitality.health.max);
        }

        public float HealthPercentage => vitality.health.current / vitality.health.max;

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
            var afterCritDamage = ApplyChanceMultiplier(damage, critical.chance, critical.damageMultiplier);

            // Apply armor reduction
            var afterArmor = Mathf.Max(afterCritDamage - defensive.armor, 0);
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
                Mathf.Min(vitality.health.current.Value + amount, vitality.health.max);
        }

        public void RestoreShield(float amount)
        {
            defensive.shield.current.Value =
                Mathf.Min(defensive.shield.current.Value + amount, defensive.shield.max);
        }


        public bool TryDodge()
        {
            return Random.value < defensive.dodgeChance;
        }
        
        public void AddExperience(int amount)
        {
            float bonusExp = amount * (1 + progression.experienceRate);
            progression.experience.Value += Mathf.RoundToInt(bonusExp);
        }
        
        public bool CanAttack(float currentTime)
        {
            return currentTime >= offensive.cooldown;
        }
        
        public float GetAttackAccuracy()
        {
            // Returns a value between 0 and 1, where 1 is perfect accuracy
            return Mathf.Clamp01(offensive.accuracy);
        }
    }
}