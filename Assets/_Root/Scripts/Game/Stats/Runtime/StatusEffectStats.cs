using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class StatusEffectStat<T>
    {
        public T amount;
        public T chance;
        public T duration;
    }

    [Serializable]
    public class StatusEffectStats<T>
    {
        public StatusEffectStat<T> knockBack;
        public StatusEffectStat<T> stun;
        public StatusEffectStat<T> slow;
        public StatusEffectStat<T> burn;
        public StatusEffectStat<T> freeze;
        public StatusEffectStat<T> poison;
        public StatusEffectStat<T> bleed;
        public StatusEffectStat<T> shock;
        public StatusEffectStat<T> fear;
        public StatusEffectStat<T> charm;
        public StatusEffectStat<T> confusion;
        public StatusEffectStat<T> sleep;
        public StatusEffectStat<T> silence;
        public StatusEffectStat<T> blind;
        public StatusEffectStat<T> curse;
        public StatusEffectStat<T> lightning;
        public StatusEffectStat<T> water;
        public StatusEffectStat<T> earth;
        public StatusEffectStat<T> fire;
        public StatusEffectStat<T> wind;
        public StatusEffectStat<T> light;
        public StatusEffectStat<T> dark;
        public StatusEffectStat<T> physical;
        public StatusEffectStat<T> magical;
    }
}