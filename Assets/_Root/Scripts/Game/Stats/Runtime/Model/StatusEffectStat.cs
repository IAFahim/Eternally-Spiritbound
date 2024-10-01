using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class StatusEffectStat<T>
    {
        public T immunity;
        public T amount;
        public T chance;
        public T duration;
    }
}