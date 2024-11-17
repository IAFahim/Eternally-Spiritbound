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
            App.Delay(_attackOriginReference.Origin.offensiveStats.lifeTime, OnTimeUp);
        }

        private void OnTimeUp()
        {
            _attackOriginReference.ReturnToPool(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            _attackOriginReference.OnAttackHit(
                new DamageInfo(other.gameObject,
                    _attackOriginReference.Origin.offensiveStats.damage, 0, 0)
            );
        }
    }
}