namespace Soul.Effects.Runtime
{
    public interface IEffectBase
    {
        string EffectType { get; }
        bool TryApply(IEffectConsumerBase consumer, float strength, float duration, float effectChance);
        void OnApply(IEffectConsumerBase consumer, float strength, float duration, float effectChance);
        void OnApplyFailed(IEffectConsumerBase consumer);
        void OnComplete();
        void CleanUp();
    }
}