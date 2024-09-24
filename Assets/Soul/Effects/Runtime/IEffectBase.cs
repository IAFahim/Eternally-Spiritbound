using UnityEngine;

namespace Soul.Effects.Runtime
{
    public interface IEffectBase
    {
        string EffectType { get; }
        GameObject EffectTarget { get; }
        IEffectConsumerBase EffectConsumer { get; }
        float EffectDuration { get; }
        float EffectStrength { get; }
        bool CanApply(GameObject target, IEffectConsumerBase consumer, out float effectStrength);
        bool TryApply(GameObject target, IEffectConsumerBase consumer);
        bool OnApply(GameObject target, IEffectConsumerBase consumer, float effectStrength);
        void OnCantApply(GameObject target, IEffectConsumerBase consumer);
        void OnComplete();
        void Cancel();
    }
}