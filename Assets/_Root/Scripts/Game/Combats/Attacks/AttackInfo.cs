using System;
using System.Collections.Generic;

namespace _Root.Scripts.Game.Combats.Attacks
{
    [Serializable]
    public class AttackInfo
    {
        public float damage = 1;
        public float speed = 10;
        public float size = 1;
        public float range = 1;
        public float lifeTime = 10f;
        public AttackType attackType;
        public List<DamageType> damageTypes = new();
    }
}