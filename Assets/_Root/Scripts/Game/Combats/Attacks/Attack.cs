using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Combats.Damages;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Attacks
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
            AttackOrigin origin, AttackInfo info, AttackType attackType,
            Action<Attack, DamageInfo> onAttackHit,
            Action<Attack, Vector3> onAttackMiss, Action<Attack, GameObject> onReturnToPool
        )
        {
            Info = info;
            Origin = origin;
            Info.attackType = attackType;
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

        public class Builder
        {
            private readonly AttackInfo _attackInfo = new();
            private AttackOrigin _attackOrigin;

            public Builder SetDamage(float damage)
            {
                _attackInfo.damage = damage;
                return this;
            }

            public Builder SetSpeed(float speed)
            {
                _attackInfo.speed = speed;
                return this;
            }

            public Builder SetSize(float size)
            {
                _attackInfo.size = size;
                return this;
            }

            public Builder SetLifeTime(float lifeTime)
            {
                _attackInfo.lifeTime = lifeTime;
                return this;
            }

            public Builder SetRange(float range)
            {
                _attackInfo.range = range;
                return this;
            }


            public Builder Origin(AttackOrigin attackOrigin)
            {
                _attackOrigin = attackOrigin;
                return this;
            }


            public Builder SetDamageType(List<DamageType> damageTypes)
            {
                _attackInfo.damageTypes = damageTypes;
                return this;
            }

            public Attack Build(
                AttackOrigin attackOrigin, AttackType attackType,
                Action<Attack, DamageInfo> onAttackHit,
                Action<Attack, Vector3> onAttackMiss, Action<Attack, GameObject> onReturnToPool
            )
            {
                return new Attack(attackOrigin, _attackInfo, attackType, onAttackHit, onAttackMiss, onReturnToPool);
            }
        }
    }
}