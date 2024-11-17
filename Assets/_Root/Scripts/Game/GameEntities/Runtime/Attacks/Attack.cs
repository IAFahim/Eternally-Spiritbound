using System;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Attacks
{
    [Serializable]
    public class Attack
    {
        [SerializeField] private AttackOrigin origin;
        [SerializeField] private WeaponComponent weaponComponent;

        public AttackOrigin Origin => origin;

        public WeaponComponent WeaponComponent => weaponComponent;

        private event Action<Attack, DamageInfo> AttackHitEvent;
        private event Action<Attack, Vector3> OnAttackMissEvent;
        public event Action<Attack, GameObject> OnReturnToPoolEvent;

        public Attack(
            AttackOrigin origin,
            WeaponComponent weaponComponent,
            Action<Attack, DamageInfo> onAttackHit,
            Action<Attack, Vector3> onAttackMiss,
            Action<Attack, GameObject> onReturnToPool
        )
        {
            this.origin = origin;
            this.weaponComponent = weaponComponent;
            AttackHitEvent = onAttackHit;
            OnAttackMissEvent = onAttackMiss;
            OnReturnToPoolEvent = onReturnToPool;
        }

        public virtual void OnAttackHit(DamageInfo damageInfo)
        {
            AttackHitEvent?.Invoke(this, damageInfo);
        }

        public virtual void OnAttackMiss(Vector3 position)
        {
            OnAttackMissEvent?.Invoke(this, position);
        }

        public virtual void ReturnToPool(GameObject attack)
        {
            OnReturnToPoolEvent?.Invoke(this, attack);
        }
    }
}