using System;
using System.Collections.Generic;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    [Serializable]
    public class AttackInfo : ICloneable
    {
        public float damage = 1;
        public float speed = 10;
        public float size = 1;
        public float range = 1;
        public float lifeTime = 10f;
        public AttackType attackType;
        public List<DamageType> damageTypes = new();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}