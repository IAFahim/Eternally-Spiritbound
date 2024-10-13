using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public struct VitalityStats<T> 
    {
        public LimitStat<T> health;
        public T size;
    }
}