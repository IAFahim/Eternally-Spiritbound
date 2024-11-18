using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public interface IWeapon
    {
        public IFocus Focus { get; }
        public Transform FirePoint { get; }
        public void PerformAttack(GameObject target);
        public void PerformAttack(Vector3 targetPosition);
        public void OnAttackHit(IBullet bullet, DamageInfo damageInfo);
        public void OnReturnToPool(IBullet bullet);

    }
}