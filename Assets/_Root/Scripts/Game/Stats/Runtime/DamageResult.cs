using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime
{
    public struct DamageResult
    {
        public float RawDamage; // Original damage before any modifications
        public float CriticalDamage; // Additional damage from critical hit
        public float ArmorDamageReduced; // Damage reduced by armor
        public float ShieldDamage; // Damage absorbed by shield
        public float HealthDamage; // Final damage to health
        public float TotalDamageDealt; // Total damage after all calculations
        public bool WasCritical; // Whether it was a critical hit
        public bool WasDodged; // Whether the attack was dodged
        public float RemainingShield; // Shield amount remaining
        public float RemainingHealth; // Health amount remaining
        public bool IsAlive; // Whether the victim is still alive
        


        public override string ToString()
        {
            return
                $"{nameof(RawDamage)}: {RawDamage}, {nameof(CriticalDamage)}: {CriticalDamage}, {nameof(ArmorDamageReduced)}: {ArmorDamageReduced}, {nameof(ShieldDamage)}: {ShieldDamage}, {nameof(HealthDamage)}: {HealthDamage}, {nameof(TotalDamageDealt)}: {TotalDamageDealt}, {nameof(WasCritical)}: {WasCritical}, {nameof(WasDodged)}: {WasDodged}, {nameof(RemainingShield)}: {RemainingShield}, {nameof(RemainingHealth)}: {RemainingHealth}";
        }
    }
}