using _Root.Scripts.Game.DamagePopups.Runtime;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
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
            DoDamage(other.transform.root);
        }

        private void DoDamage(Transform otherRootTransform)
        {
            if (otherRootTransform == _attackOrigin.weaponBaseComponent.transform.root) return;
            if (otherRootTransform.TryGetComponent<EntityStatsComponent>(out var entityStatsComponent))
            {
                entityStatsComponent.entityStats.Damage(_attackOrigin.offensiveStats.damage, out var damageResult);
                damageResult.VitimRootTransform = otherRootTransform;
                _attackOrigin.weaponBaseComponent.OnAttackHit(this, damageResult);
                damagePopup.ShowPopup(Transform.position, damageResult);
                Debug.Log("Hit");
            }
        }
    }
}