using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class RegenStat<T>
    {
        public Reactive<float> current;
        public T max;
        public T regenRate;
        public T delay;
    }
}