using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class EnableLimitStat<T> : LimitStat<T>
    {
        public bool enabled;
    }
}