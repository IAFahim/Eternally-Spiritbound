using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Stats.Runtime.Model;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    [Serializable]
    public class AttackInfo :OffensiveStats<float>, ICloneable
    {
        public AttackType attackType;
        public List<DamageType> damageTypes = new();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}