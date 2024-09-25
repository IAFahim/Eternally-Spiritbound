using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Soul.Modifiers.Runtime;

namespace _Root.Scripts.Game.Combats.Consumers
{
    [Serializable]
    public class AttackConsumer
    {
        public DamageInfluence damageInfluence;
        public AttackInfluence attackInfluence;
        public event Action<AttackInfo, DamageInput, float> OnAttackConsumed; 

        public bool DamageTaken(Modifier health, AttackInfo attackInfo, DamageInput damageInfo, out float damageTaken)
        {
            var damageStrength = damageInfluence.GetStrength(damageInfo.damageType);
            var attackStrength = attackInfluence.GetStrength(attackInfo.attackType);
            damageTaken = damageInfo.damage * damageStrength * attackStrength;
            health.Reduce(damageTaken);
            return true;
        }
    }
}