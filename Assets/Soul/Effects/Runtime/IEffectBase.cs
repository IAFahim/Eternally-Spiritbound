namespace Soul.Effects.Runtime
{
    public interface IEffectBase
    {
        string EffectType { get; }
        IEffectConsumerBase EffectConsumer { get; }
        float EffectDuration { get; }
        float EffectStrength { get; }
        bool CanApply(IEffectConsumerBase consumer, out float effectStrength);
        bool TryApply(IEffectConsumerBase consumer);
        bool OnApply(IEffectConsumerBase consumer, float effectStrength);
        void OnCantApply(IEffectConsumerBase consumer);
        void OnComplete();
        void Cancel();
    }
}