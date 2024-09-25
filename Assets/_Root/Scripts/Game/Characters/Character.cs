using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Consumers;
using _Root.Scripts.Game.Combats.Damages;
using Pancake;
using Soul.Modifiers.Runtime;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Characters
{
    public class Character : StringConstant, IHealthBase, IArmorBase, IShieldBase, IDamage
    {
        public StringConstant characterName;
        public Modifier currentHealth;
        public Modifier maxHealth;
        public Modifier currentArmor;
        public Modifier currentShield;
        public Modifier maxShield;
        public AttackConsumer attackConsumer;

        public Modifier CurrentHealth => currentHealth;
        public Modifier MaxHealth => maxHealth;
        public Modifier CurrentArmor => currentArmor;
        public Modifier CurrentShield => currentShield;
        public Modifier MaxShield => maxShield;

        // public bool TryDamage(EDamageType info, Vector3 position, float amount, out float damageTaken)
        // {
        //     // var strength = damageStrength.GetStrength(info);
        //     // float remainingDamage = amount * strength;
        //     // remainingDamage -= currentArmor.Value;
        //     //
        //     // if (remainingDamage > 0 && currentShield.Value > 0)
        //     // {
        //     //     float shieldDamage = Mathf.Min(currentShield.Value, remainingDamage);
        //     //     currentShield.Reduce(shieldDamage);
        //     //     remainingDamage -= shieldDamage;
        //     // }
        //     //
        //     // // Apply remaining damage to health
        //     // if (remainingDamage > 0)
        //     // {
        //     //     currentHealth.Reduce(remainingDamage);
        //     // }
        //     //
        //     // damageTaken = amount * strength;
        //     return true;
        // }

        public bool TryDamage(AttackInfo info, DamageInput damageInfo, out DamageInfo damageTaken)
        {
            return attackConsumer.DamageTaken(currentHealth, info, damageInfo, out var damage);
        }
    }
}