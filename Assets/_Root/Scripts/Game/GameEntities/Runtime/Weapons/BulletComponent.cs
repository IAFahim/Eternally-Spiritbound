using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class BulletComponent : MonoBehaviour
    {
        private Attack _attackOriginReference;

        public void Attack(Attack attack)
        {
            _attackOriginReference = attack;
            App.Delay(_attackOriginReference.Info.lifeTime, OnTimeUp);
        }

        public void Attack(Attack attack, GameObject target)
        {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            Attack(attack);
        }

        private void OnTimeUp()
        {
            _attackOriginReference.ReturnToPool(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            _attackOriginReference.OnAttackHit(new DamageInfo(other.gameObject, _attackOriginReference.Info.damage));
        }
    }
}