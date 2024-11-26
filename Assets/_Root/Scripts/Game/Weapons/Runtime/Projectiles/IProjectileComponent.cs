using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Projectiles
{
    public class IProjectileComponent : MonoBehaviour, IProjectile
    {
        public GameObject GameObject => gameObject;
        private AttackOrigin _attackOrigin;

        public void Init(AttackOrigin attackOrigin)
        {
            _attackOrigin = attackOrigin;
            App.Delay(_attackOrigin.offensiveStats.lifeTime, OnTimeUp);
        }

        private void Update()
        {
            transform.position += transform.forward * (_attackOrigin.offensiveStats.speed * Time.deltaTime);
        }

        private void OnTimeUp()
        {
            _attackOrigin.weaponComponent.OnReturnToPool(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<EntityStatsComponent>(out var entityStatsComponent))
            {
                entityStatsComponent.entityStats.Damage(_attackOrigin.offensiveStats.damage, out var damageResult);
                _attackOrigin.weaponComponent.OnAttackHit(this, damageResult);
            }
        }
    }
}