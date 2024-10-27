using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class EnableLimitStat<T> : LimitStat<T>
    {
        public bool enabled;
    }
}