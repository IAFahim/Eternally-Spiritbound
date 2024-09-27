using System;
using UnityEngine;

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

        public GameObject EffectTarget { get; protected set; }
        public IEffectConsumerBase EffectConsumer { get; protected set; }


        public virtual bool TryApply(GameObject target, IEffectConsumerBase consumer)
        {
            EffectTarget = target;
            EffectConsumer = consumer;
            var canApply = CanApply(target, consumer, out var effectStrength);
            if (canApply)
            {
                EffectConsumer.Apply(this);
                OnApply(EffectTarget, EffectConsumer, effectStrength);
            }
            else OnCantApply(EffectTarget, EffectConsumer);

            return canApply;
        }

        public abstract bool CanApply(GameObject target, IEffectConsumerBase consumer, out float effectStrength);
        public abstract bool OnApply(GameObject target, IEffectConsumerBase consumer, float effectStrength);
        public abstract void OnCantApply(GameObject target, IEffectConsumerBase consumer);

        public virtual void OnComplete() => EffectConsumer?.RemoveEffect(this);
        public virtual void Cancel() => EffectConsumer?.RemoveEffect(this);
        public virtual void Dispose() => Cancel();
    }
}