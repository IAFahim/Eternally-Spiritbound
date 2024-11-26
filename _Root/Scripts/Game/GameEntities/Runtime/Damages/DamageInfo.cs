using System;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    [Serializable]
    public struct DamageInfo
    {
        public GameObject damagedGameObject;
        public float damage;
        public float defended;
        public float critical;

        public DamageInfo(GameObject damagedGameObject, float damage, float defended, float critical)
        {
            this.damagedGameObject = damagedGameObject;
            this.damage = damage;
            this.defended = defended;
            this.critical = critical;
        }
    }
}