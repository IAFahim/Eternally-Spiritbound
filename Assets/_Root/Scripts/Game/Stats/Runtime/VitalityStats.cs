using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class VitalityStats<T>
    {
        public RegenStat<T> health;
        public T size;
    }
}