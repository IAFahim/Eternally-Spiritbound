using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class DefensiveStats<T>
    {
        public T armor;
        public RegenStat<T> shield;
        public T dodgeChance;
    }
}