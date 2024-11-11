using System;
using Pancake;
using Soul.Effects.Runtime;

namespace _Root.Scripts.Game.Effects.Runtime
{
    [Serializable]
    public abstract class Effect : StringConstant, IEffectBase
    {
        public string EffectType => Value;

        public virtual bool TryApply(IEffectConsumerBase consumer, float strength, float duration, float effectChance)
        {
            if (effectChance >= 1) return Apply(consumer, strength, duration, effectChance);
            if (UnityEngine.Random.value > effectChance) return Apply(consumer, strength, duration, effectChance);
            OnApplyFailed(consumer);
            return false;
        }

        private bool Apply(IEffectConsumerBase consumer, float strength, float duration, float effectChance)
        {
            var effectStrength = consumer.GetEffectMultiplier(EffectType);
            if (effectStrength > 0)
            {
                OnApply(consumer, effectStrength, duration, effectChance);
                return true;
            }

            OnApplyFailed(consumer);
            return false;
        }

        public virtual void OnApply(IEffectConsumerBase consumer, float strength, float duration, float effectChance)
        {
            consumer.Apply(new EffectInstance(this, strength, duration));
        }

        public abstract void OnApplyFailed(IEffectConsumerBase consumer);
        public abstract void OnComplete();
        public abstract void CleanUp();
    }
}