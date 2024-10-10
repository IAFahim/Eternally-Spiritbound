using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class ProgressionStats<T>
    {
        public Reactive<int> experience;
        public T experienceRate;
        public T luck;
    }
}