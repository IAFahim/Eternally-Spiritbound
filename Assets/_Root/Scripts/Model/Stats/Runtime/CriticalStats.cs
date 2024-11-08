using System;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class CriticalStats<T>
    {
        public T chance;
        public T damage;
    }
}