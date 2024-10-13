using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public struct EnableLimitStat<T>
    {
        public bool enabled;
        public LimitStat<T> limitStat;
        
        public EnableLimitStat(bool enabled, LimitStat<T> limitStat)
        {
            this.enabled = enabled;
            this.limitStat = limitStat;
        }
    }
}