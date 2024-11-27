using System;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using _Root.Scripts.Game.Weapons.Runtime.Damages;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Projectiles
{
    public class ProjectileBaseComponent : MonoBehaviour, IProjectile
    {
        [SerializeField] private DamagePopup damagePopup;
        private AttackOrigin _attackOrigin;
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;

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
            _attackOrigin.weaponBaseComponent.OnReturnToPool(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            DoDamage(other.gameObject);
        }

        private void DoDamage(GameObject other)
        {
            if (other.TryGetComponent<EntityStatsComponent>(out var entityStatsComponent))
            {
                entityStatsComponent.entityStats.Damage(_attackOrigin.offensiveStats.damage, out var damageResult);
                _attackOrigin.weaponBaseComponent.OnAttackHit(other, this, damageResult);
                damagePopup.ShowPopup(Transform.position, damageResult);
                Debug.Log("Hit");
            }
        }
    }
}