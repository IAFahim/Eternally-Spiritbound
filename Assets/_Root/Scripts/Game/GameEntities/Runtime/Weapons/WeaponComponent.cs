using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake.Common;
using Pancake.Pools;
using Soul.Interactables.Runtime;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public class WeaponComponent : MonoBehaviour, IWeapon
    {
        public int currentLevel;
        public bool fire;
        public float lastFireTime;

        [SerializeField] private WeaponAsset weaponAsset;
        [SerializeField] private bool noDelayOnFirstFire = true;
        [SerializeField] private BulletAsset bulletAsset;

        [SerializeField] private OverlapNonAlloc overlapNonAlloc;
        [SerializeField] private IntervalTicker intervalTicker;
        [SerializeField] private Transform firePoint;

        private OffensiveStats _offensiveStats;

        public IFocus FocusReference { get; private set; }
        public EntityStatsComponent EntityStatsComponent { get; private set; }

        public BulletAsset BulletAsset
        {
            get => bulletAsset;
            set => bulletAsset = value;
        }

        public WeaponAsset WeaponAsset => weaponAsset;
        public GameObject GameObject => gameObject;
        public Transform FirePoint => firePoint;


        public void Init(EntityStatsComponent entityStatsComponent, IFocus focus, int level,
            OffensiveStats playerOffensiveStats)

        {
            EntityStatsComponent = entityStatsComponent;
            FocusReference = focus;
            weaponAsset.OffensiveStatsParameterScript.TryCombine(
                currentLevel = level,
                playerOffensiveStats, out _offensiveStats
            );
        }

        private void OnEnable() => Initialize();
        private void OnDisable() => RemoveListeners();


        private void Initialize()
        {
            overlapNonAlloc.Initialize(transform);
            lastFireTime = Time.time;
            if (noDelayOnFirstFire) lastFireTime = Time.time - _offensiveStats.fireRate;
            App.AddListener(EUpdateMode.Update, OnUpdate);
            App.AddListener(EUpdateMode.FixedUpdate, OnFixedUpdate);
        }

        public void PerformAttack(GameObject other)
        {
            InitBullet(GetFromPool(), other);
        }

        public void PerformAttack(Vector3 targetPosition)
        {
            InitBullet(GetFromPool(), targetPosition);
        }

        public void OnAttackHit(IBullet bullet, DamageInfo damageInfo)
        {
            Debug.Log("Hit: " + damageInfo.damagedGameObject.name);
        }

        public void OnReturnToPool(IBullet bullet)
        {
            SharedAssetReferencePool.Return(bulletAsset, bullet.GameObject);
        }

        private void OnUpdate()
        {
            if (!overlapNonAlloc.Found()) return;
            fire = Time.time - lastFireTime >= _offensiveStats.fireRate;
            if (!fire) return;
            if (!overlapNonAlloc.TryGetClosest(out var other, out _)) return;
            PerformAttack(other.gameObject);
            fire = false;
            lastFireTime = Time.time;
        }

        private GameObject GetFromPool()
        {
            return SharedAssetReferencePool.Request(bulletAsset, firePoint.position, firePoint.rotation);
        }


        private void InitBullet(GameObject bullet, GameObject target)
        {
            bullet.GetComponent<IBullet>().Init(
                new AttackOrigin(this,
                    EntityStatsComponent,
                    _offensiveStats,
                    target,
                    transform.position,
                    target.transform.position
                )
            );
        }

        private void InitBullet(GameObject bullet, Vector3 targetPosition)
        {
            bullet.GetComponent<IBullet>().Init(
                new AttackOrigin(this,
                    EntityStatsComponent,
                    _offensiveStats,
                    null,
                    transform.position,
                    targetPosition
                )
            );
        }

        private void OnFixedUpdate()
        {
            if (intervalTicker.TryTick()) overlapNonAlloc.Perform();
        }


        private void RemoveListeners()
        {
            App.RemoveListener(EUpdateMode.Update, OnUpdate);
            App.RemoveListener(EUpdateMode.FixedUpdate, OnFixedUpdate);
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