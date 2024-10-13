using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public struct LimitStat<T> 
    {
        public Reactive<float> current;
        public T max;
    }
}