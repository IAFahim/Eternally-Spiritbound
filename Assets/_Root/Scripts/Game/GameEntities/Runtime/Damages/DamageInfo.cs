using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    public struct DamageInfo
    {
        public GameObject damagedGameObject;
        public float damage;

        public DamageInfo(GameObject damaged, float damageDealt)
        {
            damagedGameObject = damaged;
            damage = damageDealt;
        }
    }
}