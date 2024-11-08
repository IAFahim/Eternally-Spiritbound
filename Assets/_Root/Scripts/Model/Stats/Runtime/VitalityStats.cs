using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct VitalityStats<T> 
    {
        public EnableLimitStat<T> health;
        public T size;
    }
}