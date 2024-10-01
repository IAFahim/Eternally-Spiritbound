using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class RegenStat<T>
    {
        public T current;
        public T max;
        public T regenRate;
        public T delay;
    }
}