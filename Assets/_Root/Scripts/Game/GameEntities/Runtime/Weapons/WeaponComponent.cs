using System;
using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Pancake.Pools;
using Sisus.Init;
using Soul.Modifiers.Runtime;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class WeaponComponent : MonoBehaviour, IInitializable<OffensiveStats<Modifier>>, IDisposable
    {
        public Vector3 direction;
        public float normalizedRange = 1;

        public bool noDelayOnFirstFire = true;
        public Bullet strategy;
        public Bullet Strategy => strategy;

        public bool fire;
        public float lastFireTime;
        private Attack _readyAttack;
        private GameObject _spawned;

        private AddressableGameObjectPool _bulletPool;
        public OverlapNonAlloc overlapNonAlloc;
        public IntervalTicker intervalTicker;
        public OffensiveStats<float> weaponOffensiveStats;

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
            weaponOffensiveStats = strategy.GetWeaponOffensiveStats(_offensiveStats);
            overlapNonAlloc.Initialize();
            if (noDelayOnFirstFire) lastFireTime = Time.time - weaponOffensiveStats.fireRate;
        }

        private void Update()
        {
            if (!overlapNonAlloc.Found()) return;
            fire = Time.time - lastFireTime >= weaponOffensiveStats.fireRate;
            if (!fire) return;
            if (!overlapNonAlloc.TryGetClosest(out var other, out _)) return;
            direction = (other.transform.position - transform.position).normalized;
            var origin = new AttackOrigin(
                transform.parent.gameObject, other.gameObject, gameObject, _offensiveStats,
                _bulletPool, transform.position, direction, normalizedRange
            );
            Attack(origin);
            fire = false;
            lastFireTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (intervalTicker.TryTick()) overlapNonAlloc.Perform();
        }

        public void Attack(AttackOrigin origin)
        {
            weaponOffensiveStats = strategy.GetWeaponOffensiveStats(_offensiveStats);
            _spawned = _bulletPool.Request(transform.position, transform.rotation);
            _readyAttack = new Attack(origin, weaponOffensiveStats, OnAttackHit, OnAttackMiss, OnReturnToPool);
            _spawned.GetComponent<BulletComponent>().Init(_readyAttack);
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
            Debug.Log("Hit: " + arg2.Damaged.name);
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