using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class RegenStat<T>
    {
        public T value;
        public T rate;
        public T delay;
    }
}