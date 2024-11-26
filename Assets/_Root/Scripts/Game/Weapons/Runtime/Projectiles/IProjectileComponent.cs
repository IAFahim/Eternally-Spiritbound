using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using Pancake.Common;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime
{
    public class IProjectileComponent : MonoBehaviour, IInitializable<AttackOrigin>, IProjectile
    {
        public GameObject GameObject => gameObject;
        private AttackOrigin _attackOriginReference;

        public void Init(AttackOrigin attackOrigin)
        {
            _attackOriginReference = attackOrigin;
            App.Delay(_attackOriginReference.offensiveStats.lifeTime, OnTimeUp);
        }

        private void Update()
        {
            transform.position += transform.forward * (_attackOriginReference.offensiveStats.speed * Time.deltaTime);
        }

        private void OnTimeUp()
        {
            _attackOriginReference.weaponComponent.OnReturnToPool(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            _attackOriginReference.weaponComponent.OnAttackHit(this,
                new DamageInfo(other.gameObject, _attackOriginReference.offensiveStats.damage, 0, 0)
            );
        }

    }
}