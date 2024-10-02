using System;
using Pancake;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    [Serializable]
    public class AttackOrigin
    {
        public Optional<GameObject> attacker;
        public Optional<GameObject> target;
        public GameObject weapon; 
        public IObjectPool<GameObject> BulletPool;
        public Vector3 position;
        public Vector3 direction;
        public float normalizedRange;

        public AttackOrigin(GameObject attacker, GameObject target, GameObject weapon,
            IObjectPool<GameObject> bulletPool, Vector3 position, Vector3 direction, float normalizedRange)
        {
            this.attacker = attacker;
            this.target = target;
            this.weapon = weapon;
            BulletPool = bulletPool;
            this.position = position;
            this.direction = direction;
            this.normalizedRange = normalizedRange;
        }
    }
}