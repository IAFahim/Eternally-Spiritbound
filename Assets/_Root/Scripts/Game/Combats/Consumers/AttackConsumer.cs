using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Soul.Reactives.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Consumers
{
    [Serializable]
    public class AttackConsumer : MonoBehaviour
    {
        public AttackInfluence attackInfluence;
        public DamageInfluence damageInfluence;
        
        public bool DamageTaken(Reactive<float> health, Attack attack, DamageType damageType, out DamageInfo damageInfo)
        {
            var attackStrength = attackInfluence.GetStrength(attack.Info.attackType);
            var damageStrength = damageInfluence.GetStrength(damageType);
            var damageTaken = attack.Info.damage * damageStrength * attackStrength;
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