using System;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    [Serializable]
    public struct DamageInfo
    {
        public GameObject damaged;
        public float damageTaken;
        
        public DamageInfo(GameObject damaged, float damageTaken)
        {
            this.damaged = damaged;
            this.damageTaken = damageTaken;
        }
    }
}