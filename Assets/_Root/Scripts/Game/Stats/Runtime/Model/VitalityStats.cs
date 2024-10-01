using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class VitalityStats<T>
    {
        public LimitStat<T> health;
        public T size;
        
    }
}