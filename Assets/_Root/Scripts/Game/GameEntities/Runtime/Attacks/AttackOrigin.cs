using System;
using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake;
using Pancake.Pools;
using Soul.Modifiers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.GameEntities.Runtime.Attacks
{
    [Serializable]
    public struct AttackOrigin
    {
        public Optional<GameObject> target;
        public OffensiveStats<float> offensiveStats;
        public Vector3 position;
        public Vector3 direction;

        public AttackOrigin(GameObject target, OffensiveStats<float> offensiveStats,
            Vector3 position,
            Vector3 direction
        )
        {
            this.target = target;
            this.offensiveStats = offensiveStats;
            this.position = position;
            this.direction = direction;
        }
    }
}