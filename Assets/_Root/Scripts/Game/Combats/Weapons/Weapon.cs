using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon, IDisposable
    {
        public GameObject attacker;
        public LayerMask layerMask;
        public WeaponStrategyBase strategy;
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
            // Check if the weapon is ready to fire
            if (Time.time - lastFireTime >= strategy.FireRate)
            {
                fire = true;
                Attack(attacker, transform.position, transform.forward, layerMask, strategy.attackInfo, 1);
                _subComponent.Attack(readyAttack);
                fire = false; // Reset fire flag after attack
                lastFireTime = Time.time; // Update last fire time
            }
        }

        public void Attack(GameObject attacker,
            Vector3 position, Vector3 direction, LayerMask layerMask, AttackInfo attackInfo, float normalizedRange)
        {
            _spawned = _pool.Request(transform.position, transform.rotation, transform);
            var origin = new AttackOrigin(attacker, gameObject, position, direction, layerMask, normalizedRange);
            readyAttack = strategy.GetAttackBuilderFromStrategy(normalizedRange)
                .Build(origin, strategy.attackInfo.attackType, OnAttackHit, OnAttackMiss, OnReturnToPool);
            _subComponent = _spawned.GetComponent<IWeaponSubComponent>();
        }

        private void OnReturnToPool(Attack arg1, GameObject arg2)
        {
        }

        private void OnAttackMiss(Attack arg1, Vector3 arg2)
        {
        }

        private void OnAttackHit(Attack arg1, DamageInfo arg2)
        {
        }

        public void Dispose()
        {
            _pool?.Dispose();
        }
    }
}