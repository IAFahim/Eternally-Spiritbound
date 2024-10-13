using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    public struct DamageInfo
    {
        public GameObject Damaged;
        public float DamageDealt;
        
        public DamageInfo(GameObject damaged, float damageDealt)
        {
            this.Damaged = damaged;
            this.DamageDealt = damageDealt;
        }
    }
}