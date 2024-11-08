using System;
using _Root.Scripts.Game.Stats.Runtime.Model;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class DefensiveStats<T> 
    {
        public T armor;
        public LimitStat<T> shield;
        public T dodgeChance;
    }
}