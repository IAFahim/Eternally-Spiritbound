using System;

namespace Soul.Effects.Runtime
{
    [Serializable]
    public class EffectInstance : IDisposable
    {
        public IEffectBase BaseEffect;
        public float strength;
        public float duration;

        public EffectInstance(IEffectBase baseEffect, float strength, float duration)
        {
            BaseEffect = baseEffect;
            this.strength = strength;
            this.duration = duration;
        }

        public bool KeepUpdating(float deltaTime)
        {
            duration -= deltaTime;
            if (duration > 0) return true;
            BaseEffect.OnComplete();
            return false;
        }

        public void Dispose() => BaseEffect.CleanUp();
    }
}