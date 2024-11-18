using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct VitalityStats
    {
        public LimitStat health;
        public float size;

        public VitalityStats Combine(VitalityStats other)
        {
            return new VitalityStats
            {
                health = new LimitStat
                {
                    current = health.current,
                    max = health.max
                },
                size = size + other.size
            };
        }
    }
}