using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Model.Stats.Runtime;
using Sisus.Init;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public interface IWeapon : IInitializable<EntityStatsComponent, IFocus, int, OffensiveStats>
    {
        public WeaponAsset WeaponAsset { get; }
        public IFocus FocusReference { get; }
        public GameObject GameObject { get; }
        public Transform FirePoint { get; }
        public void PerformAttack(GameObject target);
        public void PerformAttack(Vector3 targetPosition);
        public void OnAttackHit(IBullet bullet, DamageInfo damageInfo);
        public void OnReturnToPool(IBullet bullet);
    }
}