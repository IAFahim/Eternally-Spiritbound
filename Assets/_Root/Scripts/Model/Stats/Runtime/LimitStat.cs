using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class LimitStat<T>
    {
        public Reactive<float> current;
        public T max;
    }
}