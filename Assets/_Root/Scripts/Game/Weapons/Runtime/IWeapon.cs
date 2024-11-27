using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Weapons.Runtime.Projectiles;
using _Root.Scripts.Model.Stats.Runtime;
using Sisus.Init;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime
{
    public interface IWeapon : IInitializable<EntityStatsComponent, IFocus, int, int>
    {
        public WeaponAsset WeaponAsset { get; }
        public IFocus FocusReference { get; }
        public GameObject GameObject { get; }
        public Transform FirePoint { get; }
        public void PerformAttack(GameObject target);
        public void PerformAttack(Vector3 targetPosition);
        public void OnAttackHit(GameObject victim, IProjectile iProjectile, DamageResult damageResult);
        public void OnReturnToPool(IProjectile iProjectile);
    }
}