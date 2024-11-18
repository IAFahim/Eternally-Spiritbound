using System;
using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake.Pools;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class WeaponComponent : MonoBehaviour
    {
        public int level;

        [SerializeField] private AssetScript weaponAsset;
        [SerializeField] private bool noDelayOnFirstFire = true;
        [SerializeField] private Bullet bulletScript;
        [SerializeField] private OffensiveStatsParameterScript offensiveStatsParameterScript;


        public bool fire;
        public float lastFireTime;
        private Attack attackInstence;
        private GameObject _spawned;

        public OverlapNonAlloc overlapNonAlloc;
        public IntervalTicker intervalTicker;
        public OffensiveStats offensiveStats;
        public Bullet BulletScript => bulletScript;

        private void OnEnable()
        {
            Initialize(level);
        }


        public void Initialize(int currentLevel)
        {
            offensiveStatsParameterScript.TryGetParameter(level = currentLevel, out offensiveStats);
            overlapNonAlloc.Initialize(transform);
            lastFireTime = Time.time;
            if (noDelayOnFirstFire) lastFireTime = Time.time - offensiveStats.fireRate;
        }

        private void Update()
        {
            if (!overlapNonAlloc.Found()) return;
            fire = Time.time - lastFireTime >= offensiveStats.fireRate;
            if (!fire) return;
            if (!overlapNonAlloc.TryGetClosest(out var other, out _)) return;

            Attack(other);

            fire = false;
            lastFireTime = Time.time;
        }

        public void Attack(Collider other)
        {
            var direction = (other.transform.position - transform.position).normalized;
            var attackOrigin = new AttackOrigin(other.gameObject, offensiveStats, transform.position, direction);
            Attack(attackOrigin, other.gameObject);
        }

        public void Attack(AttackOrigin origin, GameObject target)
        {
            _spawned = SharedAssetReferencePool.Request(bulletScript, transform.position, transform.rotation);
            attackInstence = new Attack(origin, this, OnAttackHit, OnAttackMiss, OnReturnToPool);
            _spawned.GetComponent<BulletComponent>().Attack(attackInstence);
        }

        public void Attack(Vector3 direction)
        {
            var attackOrigin = new AttackOrigin(null, offensiveStats, transform.position, direction);
            Attack(attackOrigin);
        }


        public void Attack(AttackOrigin origin)
        {
        }


        private void FixedUpdate()
        {
            if (intervalTicker.TryTick()) overlapNonAlloc.Perform();
        }


        private void OnAttackMiss(Attack arg1, Vector3 arg2)
        {
            Debug.Log("Miss: " + arg2);
        }


        private void OnReturnToPool(Attack attack, GameObject bulletGameObject)
        {
            SharedAssetReferencePool.Return(attack.WeaponComponent.bulletScript, bulletGameObject);
        }


        private void OnAttackHit(Attack arg1, DamageInfo arg2)
        {
            Debug.Log("Hit: " + arg2.damagedGameObject.name);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 10);
            overlapNonAlloc.DrawGizmos(Color.red, Color.green);
        }
#endif
    }
}