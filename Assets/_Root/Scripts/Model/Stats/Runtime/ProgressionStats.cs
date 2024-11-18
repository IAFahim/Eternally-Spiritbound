using System;
using Soul.Reactives.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct ProgressionStats
    {
        public Reactive<int> experience;
        public float experienceRate;
        public float luck;
    }
}