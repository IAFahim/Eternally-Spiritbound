using System;
using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Combats.Runtime.Damages;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Pancake.Pools;
using Sirenix.OdinInspector;
using Sisus.Init;
using Soul.Modifiers.Runtime;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public class WeaponComponent : MonoBehaviour, IInitializable<OffensiveStats<Modifier>>, IDisposable
    {
        public Vector3 direction;
        public Bullet strategy;
        public float normalizedRange = 1;
        public Bullet Strategy => strategy;

        public bool fire;
        public float lastFireTime;
        private Attack readyAttack;
        private GameObject _spawned;

        private AddressableGameObjectPool _bulletPool;
        public OverlapNonAlloc overlapNonAlloc;
        public IntervalTicker intervalTicker;

        private void OnEnable()
        {
            Initialize();
            lastFireTime = Time.time;
        }

        private OffensiveStats<Modifier> _offensiveStats;

        public void Init(OffensiveStats<Modifier> firstArgument)
        {
            _offensiveStats = firstArgument;
        }

        public void Initialize()
        {
            _bulletPool = new AddressableGameObjectPool(strategy.assetReferenceGameObject);
            overlapNonAlloc.Init(1);
        }

        private void Update()
        {
            if (overlapNonAlloc.Found())
            {
                fire = Time.time - lastFireTime >= strategy.FireRate;
                if (fire)
                {
                    var other = overlapNonAlloc.Colliders[0].gameObject;
                    direction = (other.transform.position - transform.position).normalized;
                    var origin = new AttackOrigin(
                        transform.parent.gameObject, other, gameObject, _offensiveStats,
                        _bulletPool, transform.position, direction, normalizedRange
                    );
                    Attack(origin, strategy.offensiveStats);
                    fire = false;
                    lastFireTime = Time.time;
                }
            }
        }

        private void FixedUpdate()
        {
            if (intervalTicker.TryTick()) overlapNonAlloc.PerformOverlap();
        }

        public void Attack(AttackOrigin origin, OffensiveStats<float> attackInfo)
        {
            _spawned = _bulletPool.Request(transform.position, transform.rotation);
            readyAttack = new Attack(origin, attackInfo, OnAttackHit, OnAttackMiss, OnReturnToPool);
            _spawned.GetComponent<BulletComponent>().Init(readyAttack);
        }

        private void OnAttackMiss(Attack arg1, Vector3 arg2)
        {
            Debug.Log("Miss: " + arg2);
        }


        private void OnReturnToPool(Attack arg1, GameObject arg2)
        {
            _bulletPool.Return(arg2);
        }


        private void OnAttackHit(Attack arg1, DamageInfo arg2)
        {
            Debug.Log("Hit: " + arg2.damaged.name);
        }

        public void Dispose()
        {
            _bulletPool?.Dispose();
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction);
            overlapNonAlloc.DrawGizmos(Color.red, Color.green);
        }
#endif
    }
}