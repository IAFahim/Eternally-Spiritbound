using System;
using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Combats.Runtime.Damages;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Pancake.Pools;
using Sisus.Init;
using Soul.Modifiers.Runtime;
using Soul.OverlapSugar.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public class WeaponComponent : MonoBehaviour<OffensiveStats<Modifier>>, IDisposable
    {
        public Vector3 direction;
        public Bullet strategy;
        public float normalizedRange;
        public Bullet Strategy => strategy;

        public bool fire;
        public float lastFireTime;
        private Attack readyAttack;
        private GameObject _spawned;

        private AddressableGameObjectPool _bulletPool;
        public OverlapSettings overlapSettings;
        public int skipFrame = 5;
        private int _skippedFrame;

        private void OnEnable()
        {
            Initialize();
            lastFireTime = Time.time;
            _skippedFrame = 0;
        }
        
        private OffensiveStats<Modifier> _offensiveStats;
        protected override void Init(OffensiveStats<Modifier> firstArgument)
        {
            _offensiveStats = firstArgument;
        }

        public void Initialize()
        {
            _bulletPool = new AddressableGameObjectPool(strategy.assetReferenceGameObject);
            overlapSettings.Init(1);
        }

        private void Update()
        {
            if (overlapSettings.foundSize != 0)
            {
                fire = Time.time - lastFireTime >= strategy.FireRate;
                var other = overlapSettings.Colliders[0].gameObject;
                if (other != null)
                {
                    transform.GetPositionAndRotation(out var position, out var rotation);
                    direction = (other.transform.position - position).normalized;
                    transform.rotation =
                        Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);

                    if (fire)
                    {
                        var origin = new AttackOrigin(
                            transform.parent.gameObject, other, gameObject, _bulletPool, transform.position,
                            direction, normalizedRange
                        );
                        Attack(origin, (AttackInfo)strategy.attackInfo.Clone());

                        fire = false;
                        lastFireTime = Time.time;
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            _skippedFrame++;
            if (_skippedFrame < skipFrame) return;
            _skippedFrame = 0;

            int foundSize = overlapSettings.PerformOverlap(out _);
        }

        public void Attack(AttackOrigin origin, AttackInfo attackInfo)
        {
            _spawned = _bulletPool.Request(transform.position, transform.rotation, transform);
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
            overlapSettings.DrawGizmos(Color.red, Color.green);
        }
#endif

        
    }
}