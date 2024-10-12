using System;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Pancake;
using Pancake.Pools;
using Soul.Modifiers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Attacks
{
    [Serializable]
    public class AttackOrigin
    {
        public Optional<GameObject> attacker;
        public Optional<OffensiveStats<Modifier>> attackerOffensiveStats;
        public Optional<GameObject> target;
        public GameObject weaponComponent;
        public IObjectPool<GameObject> BulletPool;
        public Vector3 position;
        public Vector3 direction;
        public float normalizedRange;

        public AttackOrigin(
            GameObject attacker, GameObject target, GameObject weaponComponent, OffensiveStats<Modifier> attackerOffensiveStats,
            IObjectPool<GameObject> bulletPool, Vector3 position, Vector3 direction, float normalizedRange
        )
        {
            this.attacker = attacker;
            this.attackerOffensiveStats = attackerOffensiveStats;
            this.target = target;
            this.weaponComponent = weaponComponent;
            BulletPool = bulletPool;
            this.position = position;
            this.direction = direction;
            this.normalizedRange = normalizedRange;
        }
    }
}