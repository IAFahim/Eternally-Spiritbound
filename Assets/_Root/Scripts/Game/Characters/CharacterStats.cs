using _Root.Scripts.Game.Damages;
using _Root.Scripts.Game.Enums;
using Soul.Modifiers.Runtime;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Characters
{
    public class CharacterStats : MonoBehaviour, IHealthBase, IArmorBase, IShieldBase, IDamage
    {
        public Modifier currentHealth;
        public Modifier maxHealth;
        public Modifier currentArmor;
        public Modifier currentShield;
        public Modifier maxShield;
        public DamageStrength damageStrength;

        public Modifier CurrentHealth => currentHealth;
        public Modifier MaxHealth => maxHealth;
        public Modifier CurrentArmor => currentArmor;
        public Modifier CurrentShield => currentShield;
        public Modifier MaxShield => maxShield;

        public bool TryDamage(EDamageType type, Vector3 position, float amount, out float damageTaken)
        {
            var strength = damageStrength.GetStrength(type);
            float remainingDamage = amount * strength;
            remainingDamage -= currentArmor.Value;

            if (remainingDamage > 0 && currentShield.Value > 0)
            {
                float shieldDamage = Mathf.Min(currentShield.Value, remainingDamage);
                currentShield.Reduce(shieldDamage);
                remainingDamage -= shieldDamage;
            }

            // Apply remaining damage to health
            if (remainingDamage > 0)
            {
                currentHealth.Reduce(remainingDamage);
            }

            damageTaken = amount * strength;
            return true;
        }
    }
}