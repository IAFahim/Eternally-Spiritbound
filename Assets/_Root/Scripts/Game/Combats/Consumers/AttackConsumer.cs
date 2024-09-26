using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Soul.Modifiers.Runtime;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Game.Combats.Consumers
{
    [Serializable]
    public class AttackConsumer
    {
        public DamageInfluence damageInfluence;
        public AttackInfluence attackInfluence;
        public event Action<AttackInfo, DamageInfo, float> OnAttackConsumed;

        public bool DamageTaken(Reactive<float> health, AttackInfo attackInfo, out DamageInfo damageInfo)
        {
            var attackStrength = attackInfluence.GetStrength(attackInfo.attackType);
            var damageStrength = damageInfluence.GetStrength(attackInfo.damageType);
            var damageTaken = attackInfo.damage * damageStrength * attackStrength;
            health.Value -= damageTaken;
            damageInfo = new DamageInfo
            {
                damaged = null,
                damageTaken = damageTaken
            };
            return true;
        }
    }
}