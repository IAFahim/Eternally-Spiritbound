using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public struct VitalityStats<T> 
    {
        public EnableLimitStat<T> health;
        public T size;
    }
}