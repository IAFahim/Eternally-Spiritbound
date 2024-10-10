using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class ProgressionStats<T>
    {
        public T experienceRate;
        public T luck;
    }
}