using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon, IDisposable
    {
        public LayerMask layerMask;
        public Vector3 direction;
        public WeaponStrategyBase strategy;
        public float normalizedRange;
        public WeaponStrategyBase Strategy => strategy;
        public bool fire;
        public float lastFireTime;
        private Attack readyAttack;
        private GameObject _spawned;
        private IWeaponSubComponent _subComponent;

        private AddressableGameObjectPool _pool;

        private void OnEnable()
        {
            Initialize();
            lastFireTime = Time.time;
        }

        public void Initialize()
        {
            _pool = new AddressableGameObjectPool(strategy.assetReferenceGameObject);
        }

        private void Update()
        {
            if (Time.time - lastFireTime >= strategy.FireRate)
            {
                fire = true;

                var origin = new AttackOrigin(
                    transform.parent.gameObject, gameObject, transform.position,
                    direction, layerMask, normalizedRange
                );
                Attack(origin, strategy.attackInfo);
                _subComponent.Attack(readyAttack);
                
                fire = false;
                lastFireTime = Time.time;
            }
        }

        public void Attack(AttackOrigin origin, AttackInfo attackInfo)
        {
            _spawned = _pool.Request(transform.position, transform.rotation, transform);

            readyAttack = new Attack(origin, attackInfo, OnAttackHit, OnAttackMiss,
                OnReturnToPool);
            _subComponent = _spawned.GetComponent<IWeaponSubComponent>();
        }

        private void OnAttackMiss(Attack arg1, Vector3 arg2)
        {
            Debug.Log("Miss: " + arg2);
        }


        private void OnReturnToPool(Attack arg1, GameObject arg2)
        {
            _pool.Return(arg2);
        }


        private void OnAttackHit(Attack arg1, DamageInfo arg2)
        {
            Debug.Log("Hit: " + arg2.damaged.name);
        }

        public void Dispose()
        {
            _pool?.Dispose();
        }
    }
}