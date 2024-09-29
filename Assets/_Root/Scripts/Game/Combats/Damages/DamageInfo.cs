using System;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Damages
{
    [Serializable]
    public class DamageInfo
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