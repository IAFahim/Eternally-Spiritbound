using System;
using _Root.Scripts.Game.Combats.Attacks;
using _Root.Scripts.Game.Combats.Damages;
using Pancake.Pools;
using Soul.OverlapSugar.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon, IDisposable
    {
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
        public OverlapSettings overlapSettings;
        public int skipFrame = 5;
        private int _skippedFrame;

        private void OnEnable()
        {
            Initialize();
            lastFireTime = Time.time;
            _skippedFrame = 0;
        }

        public void Initialize()
        {
            _pool = new AddressableGameObjectPool(strategy.assetReferenceGameObject);
            overlapSettings.Init(1);
        }

        private void Update()
        {
            if (overlapSettings.foundSize > 0) return;
            if (Time.time - lastFireTime >= strategy.FireRate)
            {
                fire = true;

                var origin = new AttackOrigin(
                    transform.parent.gameObject, gameObject, transform.position,
                    direction, normalizedRange
                );
                Attack(origin, strategy.attackInfo);
                _subComponent.Attack(readyAttack);

                fire = false;
                lastFireTime = Time.time;
            }
        }

        private void FixedUpdate()
        {
            _skippedFrame++;
            if (_skippedFrame < skipFrame) return;
            _skippedFrame = 0;
            
            int foundSize = overlapSettings.PerformOverlap(out var foundColliders);
            if (foundSize > 0) direction = (foundColliders[0].transform.position - transform.position).normalized;
        }

        public void Attack(AttackOrigin origin, AttackInfo attackInfo)
        {
            _spawned = _pool.Request(transform.position, transform.rotation, transform);
            readyAttack = new Attack(origin, attackInfo, OnAttackHit, OnAttackMiss, OnReturnToPool);
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


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction);
            overlapSettings.DrawGizmos(Color.red, Color.green);
        }
#endif
    }
}