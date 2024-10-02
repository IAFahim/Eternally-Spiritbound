using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Combats.Runtime.Damages;
using Pancake.Common;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public class Bullet : MonoBehaviour, IInitializable<Attack>
    {
        private Attack attackReference;
        public void Init(Attack firstArgument)
        {
            attackReference = firstArgument;
            App.Delay(attackReference.Info.lifeTime, OnTimeUp);
        }
        
        private void OnTimeUp()
        {
            attackReference.ReturnToPool(gameObject);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            attackReference.OnAttackHit(new DamageInfo(other.gameObject, attackReference.Info.damage));
        }
    }
}