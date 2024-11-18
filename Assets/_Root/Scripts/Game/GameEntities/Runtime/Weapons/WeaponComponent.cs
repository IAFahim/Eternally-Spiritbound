using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake.Pools;
using Sisus.Init;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class WeaponComponent : MonoBehaviour, IInitializable<int, OffensiveStats>
    {
        public int currentLevel;
        [SerializeField] private Weapon weaponAsset;
        [SerializeField] private bool noDelayOnFirstFire = true;
        [SerializeField] private BulletScript bulletScript;
        public bool fire;
        public float lastFireTime;
        private GameObject _spawned;

        public OverlapNonAlloc overlapNonAlloc;
        public IntervalTicker intervalTicker;
        public BulletScript BulletScript => bulletScript;
        private OffensiveStats _offensiveStats;

        private void OnEnable()
        {
            Initialize();
        }

        public void Init(int level, OffensiveStats playerOffensiveStats)
        {
            weaponAsset.OffensiveStatsParameterScript.TryCombine(currentLevel = level, playerOffensiveStats,
                out _offensiveStats);
        }


        private void Initialize()
        {
            overlapNonAlloc.Initialize(transform);
            lastFireTime = Time.time;
            if (noDelayOnFirstFire) lastFireTime = Time.time - _offensiveStats.fireRate;
        }

        private void Update()
        {
            if (!overlapNonAlloc.Found()) return;
            fire = Time.time - lastFireTime >= _offensiveStats.fireRate;
            if (!fire) return;
            if (!overlapNonAlloc.TryGetClosest(out var other, out _)) return;
            PerformAttack(other);
            fire = false;
            lastFireTime = Time.time;
        }

        public void PerformAttack(Collider other)
        {
            _spawned = SharedAssetReferencePool.Request(bulletScript, transform.position, transform.rotation);
            InitBullet(other.gameObject);
        }

        private void InitBullet(GameObject other)
        {
            _spawned.GetComponent<BulletComponent>().Init(
                new AttackOrigin(this,
                    _offensiveStats,
                    other,
                    transform.position,
                    other.transform.position
                )
            );
        }


        private void FixedUpdate()
        {
            if (intervalTicker.TryTick()) overlapNonAlloc.Perform();
        }


        public void OnAttackHit(BulletComponent arg1, DamageInfo arg2)
        {
            Debug.Log("Hit: " + arg2.damagedGameObject.name);
        }

        public void OnReturnToPool(BulletComponent bulletComponent)
        {
            SharedAssetReferencePool.Return(bulletScript, bulletComponent.gameObject);
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