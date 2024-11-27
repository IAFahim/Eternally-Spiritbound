using System;
using _Root.Scripts.Game.Stats.Runtime;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Weapons.Runtime.Attacks
{
    [Serializable]
    public struct AttackOrigin
    {
        [FormerlySerializedAs("weaponComponent")] public WeaponBaseComponent weaponBaseComponent;
        public EntityStatsComponent entityStatsComponent;
        public OffensiveStats offensiveStats;


        public Optional<GameObject> target;
        public Vector3 originPosition;
        public Vector3 targetPosition;

        public AttackOrigin(WeaponBaseComponent weaponBaseComponent, EntityStatsComponent entityStatsComponent,
            OffensiveStats offensiveStats, GameObject target, Vector3 originPosition, Vector3 targetPosition)
        {
            this.weaponBaseComponent = weaponBaseComponent;
            this.entityStatsComponent = entityStatsComponent;
            this.offensiveStats = offensiveStats;
            this.target = new Optional<GameObject>(target, target);
            this.originPosition = originPosition;
            this.targetPosition = targetPosition;
        }
    }
}