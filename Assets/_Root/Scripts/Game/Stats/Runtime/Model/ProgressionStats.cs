using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class ProgressionStats<T>
    {
        public T experience;
        public T luck;
    }
}