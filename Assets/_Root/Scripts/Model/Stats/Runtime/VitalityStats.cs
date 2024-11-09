using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct VitalityStats<T> 
    {
        public LimitStat<T> health;
        public T size;
    }
}