using System;

namespace Soul.Effects.Runtime
{
    [Serializable]
    public abstract class Effect : IEffectBase, IDisposable
    {
        public float baseDuration;
        public float baseStrength;
        public abstract float EffectDuration { get; }
        public abstract float EffectStrength { get; }
        public abstract string EffectType { get; }

        public IEffectConsumerBase EffectConsumer { get; protected set; }


        public virtual bool TryApply(IEffectConsumerBase consumer)
        {
            EffectConsumer = consumer;
            var canApply = CanApply(consumer, out var effectStrength);
            if (canApply)
            {
                EffectConsumer.Apply(this);
                OnApply(EffectConsumer, effectStrength);
            }
            else OnCantApply(EffectConsumer);

            return canApply;
        }

        public abstract bool CanApply(IEffectConsumerBase consumer, out float effectStrength);
        public abstract bool OnApply(IEffectConsumerBase consumer, float effectStrength);
        public abstract void OnCantApply(IEffectConsumerBase consumer);

        public virtual void OnComplete() => EffectConsumer?.RemoveEffect(this);
        public virtual void Cancel() => EffectConsumer?.RemoveEffect(this);
        public virtual void Dispose() => Cancel();
    }
}