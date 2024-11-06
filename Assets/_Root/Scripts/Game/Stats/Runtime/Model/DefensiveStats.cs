using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class DefensiveStats<T> 
    {
        public T armor;
        public LimitStat<T> shield;
        public T dodgeChance;
    }
}