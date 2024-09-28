using _Root.Scripts.Game.Combats.Damages;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Attacks
{
    public abstract class AttackType : ScriptableObject
    {
        public StringConstant type;
        
        public void Execute(Attack attack, GameObject target)
        {
            Damage damage = CalculateDamage(attack, target);
            DamageInfo damageInfo = ApplyDamage(damage, target);
            attack.OnAttackHit(damageInfo);
        }
        protected abstract DamageInfo ApplyDamage(Damage damage, GameObject target);
        protected abstract Damage CalculateDamage(Attack attack, GameObject target);
    }
}