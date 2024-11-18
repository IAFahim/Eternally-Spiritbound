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
        public WeaponComponent weaponComponent;
        public OffensiveStats offensiveStats;
        
        public Optional<GameObject> target;
        public Vector3 originPosition;
        public Vector3 targetPosition;

        public AttackOrigin(WeaponComponent weaponComponent, OffensiveStats offensiveStats, Optional<GameObject> target, Vector3 originPosition, Vector3 targetPosition)
        {
            this.weaponComponent = weaponComponent;
            this.offensiveStats = offensiveStats;
            this.target = target;
            this.originPosition = originPosition;
            this.targetPosition = targetPosition;
        }
    }
}