using System;
using _Root.Scripts.Game.Utils.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.Stats.Runtime
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


        public void Damage(float damage, out DamageResult damageResult)
        {
            damageResult = new DamageResult
            {
                RawDamage = damage,
                RemainingShield = defensive.shield.current.Value,
                RemainingHealth = vitality.health.current.Value
            };

            // Check for dodge
            if (TryDodge())
            {
                damageResult.WasDodged = true;
                return; // Dodged
            }

            // Calculate critical hit
            bool wasCritical = GameMath.ApplyChanceMultiplier(
                damage,
                critical.chance,
                critical.damageMultiplier,
                out var totalDamage
            );

            damageResult.WasCritical = wasCritical;
            damageResult.CriticalDamage = totalDamage - damage;


            // Apply armor reduction
            float afterArmor = Mathf.Max(totalDamage - defensive.armor, 0);
            float armorDamage = totalDamage - afterArmor;


            // Handle shield damage
            float remainingDamage = afterArmor;
            float shieldDamage = 0;

            if (defensive.shield.current.Value > 0)
            {
                shieldDamage = Mathf.Min(defensive.shield.current.Value, remainingDamage);
                defensive.shield.current.Value -= shieldDamage;
                remainingDamage -= shieldDamage;
                damageResult.RemainingShield = defensive.shield.current.Value;
            }

            damageResult.ShieldDamage = shieldDamage;

            // Apply final damage to health
            if (remainingDamage > 0)
            {
                vitality.health.current.Value -= remainingDamage;
                damageResult.HealthDamage = remainingDamage;
                damageResult.RemainingHealth = vitality.health.current.Value;
            }

            // Calculate total damage dealt
            damageResult.TotalDamageDealt = shieldDamage + damageResult.HealthDamage;
            damageResult.IsAlive = vitality.health.current.Value <= 0;
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