using System;
using _Root.Scripts.Game.Combats.Runtime.Damages;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    [Serializable]
    public class Attack
    {
        [SerializeField] private AttackInfo info;
        [SerializeField] private AttackOrigin origin;

        public AttackInfo Info
        {
            get => info;
            private set => info = value;
        }

        public AttackOrigin Origin
        {
            get => origin;
            private set => origin = value;
        }

        private event Action<Attack, DamageInfo> AttackHitEvent;
        private event Action<Attack, Vector3> OnAttackMissEvent;
        public event Action<Attack, GameObject> OnReturnToPoolEvent;

        public Attack(
            AttackOrigin origin, AttackInfo info,
            Action<Attack, DamageInfo> onAttackHit,
            Action<Attack, Vector3> onAttackMiss, 
            Action<Attack, GameObject> onReturnToPool
        )
        {
            Info = info;
            Origin = origin;
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