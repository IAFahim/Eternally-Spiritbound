using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct LimitStat
    {
        public Reactive<float> current;
        public float max;
    }
}