using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using _Root.Scripts.Game.Weapons.Runtime.Projectiles;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake.Common;
using Pancake.Pools;
using Soul.Interactables.Runtime;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime
{
    public class WeaponBaseComponent : MonoBehaviour, IWeapon
    {
        public int currentLevel;
        public float lastFireTime;

        [SerializeField] private WeaponAsset weaponAsset;
        [SerializeField] private bool noDelayOnFirstFire = true;
        [SerializeField] private OverlapNonAlloc overlapNonAlloc;
        [SerializeField] private IntervalTicker intervalTicker;
        [SerializeField] private Transform firePoint;

        private ProjectileAsset _projectileAsset;
        private OffensiveStats _offensiveStats;

        public IFocus FocusReference { get; private set; }
        public EntityStatsComponent EntityStatsComponent { get; private set; }


        public WeaponAsset WeaponAsset => weaponAsset;
        public GameObject GameObject => gameObject;
        public Transform FirePoint => firePoint;

        private OffensiveStats CombineOffensiveStats()
        {
            return _offensiveStats = weaponAsset.OffensiveStatsParameters.Combine(
                currentLevel,
                EntityStatsComponent.entityStats.offensive
            );
        }


        public void Init(EntityStatsComponent entityStatsComponent, IFocus focus,
            int level,
            int selectedProjectileIndex
        )
        {
            EntityStatsComponent = entityStatsComponent;
            FocusReference = focus;
            _offensiveStats = weaponAsset.OffensiveStatsParameters.Combine(
                currentLevel = level,
                entityStatsComponent.entityStats.offensive
            );
            _projectileAsset = weaponAsset.SupportedProjectileAssets[selectedProjectileIndex];
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

        public void OnAttackHit(IProjectile iProjectile, DamageResult damageResult)
        {
            Debug.Log("Hit: " + damageResult.EntityStatsComponent.name);
        }

        public void OnReturnToPool(IProjectile iProjectile)
        {
            SharedAssetReferencePool.Return(_projectileAsset, iProjectile.GameObject);
        }

        private void OnUpdate()
        {
            if (!overlapNonAlloc.Found()) return;
            if (!(Time.time - lastFireTime >= _offensiveStats.fireRate)) return;
            if (!overlapNonAlloc.TryGetClosest(out var other, out _)) return;
            PerformAttack(other.gameObject);
        }
        

        private GameObject GetFromPool()
        {
            return SharedAssetReferencePool.Request(_projectileAsset, firePoint.position, firePoint.rotation);
        }


        private void InitBullet(GameObject bullet, GameObject target)
        {
            bullet.GetComponent<IProjectile>().Init(
                new AttackOrigin(this,
                    EntityStatsComponent,
                    CombineOffensiveStats(),
                    target,
                    transform.position,
                    target.transform.position
                )
            );
            lastFireTime = Time.time;
        }

        private void InitBullet(GameObject bullet, Vector3 targetPosition)
        {
            bullet.GetComponent<IProjectile>().Init(
                new AttackOrigin(this,
                    EntityStatsComponent,
                    CombineOffensiveStats(),
                    null,
                    transform.position,
                    targetPosition
                )
            );
            lastFireTime = Time.time;
        }

        protected virtual void OnFixedUpdate()
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