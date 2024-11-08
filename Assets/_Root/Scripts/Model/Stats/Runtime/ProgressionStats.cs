using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct ProgressionStats<T>
    {
        public Reactive<int> experience;
        public T experienceRate;
        public T luck;
    }
}