﻿using System;
using _Root.Scripts.Model.Stats.Runtime;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Attacks
{
    [Serializable]
    public struct AttackOrigin
    {
        public WeaponComponent weaponComponent;
        public EntityStatsComponent entityStatsComponent;
        public OffensiveStats offensiveStats;


        public Optional<GameObject> target;
        public Vector3 originPosition;
        public Vector3 targetPosition;

        public AttackOrigin(WeaponComponent weaponComponent, EntityStatsComponent entityStatsComponent,
            OffensiveStats offensiveStats, GameObject target, Vector3 originPosition, Vector3 targetPosition)
        {
            this.weaponComponent = weaponComponent;
            this.entityStatsComponent = entityStatsComponent;
            this.offensiveStats = offensiveStats;
            this.target = new Optional<GameObject>(target, target);
            this.originPosition = originPosition;
            this.targetPosition = targetPosition;
        }
    }
}