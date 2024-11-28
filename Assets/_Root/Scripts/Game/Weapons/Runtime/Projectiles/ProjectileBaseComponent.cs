using _Root.Scripts.Game.DamagePopups.Runtime;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using Pancake.Common;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Projectiles
{
    public abstract class ProjectileBaseComponent : MonoBehaviour, IProjectile
    {
        [SerializeField] private DamagePopup damagePopup;
        protected AttackOrigin AttackOrigin;
        protected int Penetration;
        
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        
        public virtual void Init(AttackOrigin attackOrigin)
        {
            AttackOrigin = attackOrigin;
            App.Delay(AttackOrigin.offensiveStats.lifeTime, OnTimeUp, OnUpdate);
        }

        protected abstract void OnUpdate(float timePassed);


        protected virtual void OnTimeUp()
        {
            AttackOrigin.weaponBaseComponent.OnReturnToPool(this);
        }
        

        protected void DamageInformPentrationCounterUp(Transform otherRootTransform)
        {
            if (otherRootTransform == AttackOrigin.weaponBaseComponent.transform.root) return;
            if (otherRootTransform.TryGetComponent<EntityStatsComponent>(out var entityStatsComponent))
            {
                InformWeapon(entityStatsComponent);
                PenetrationCheck();
            }
        }

        protected void InformWeapon(EntityStatsComponent entityStatsComponent)
        {
            entityStatsComponent.entityStats.Damage(AttackOrigin.offensiveStats.damage, out var damageResult);
            damageResult.EntityStatsComponent = entityStatsComponent;
            AttackOrigin.weaponBaseComponent.OnAttackHit(this, damageResult);
            damagePopup.ShowPopup(Transform.position, damageResult);
        }

        protected void PenetrationCheck()
        {
            if (Penetration < AttackOrigin.offensiveStats.penetration) Penetration++;
            else AttackOrigin.weaponBaseComponent.OnReturnToPool(this);
        }
    }
}