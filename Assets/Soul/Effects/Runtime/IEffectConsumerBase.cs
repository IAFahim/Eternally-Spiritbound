using System.Collections.Generic;

namespace Soul.Effects.Runtime
{
    public interface IEffectConsumerBase
    {
        int ActiveEffectCount { get; }
        int EffectCount(string effectType);
        IEnumerator<IEffectBase> GetEffects(string effectType);

        public float StatMultiplier { get; set; }
        float GetEffectMultiplier(string effectType);
        bool CanApplyEffect(string effectType, out float effectStrength);
        void Apply(IEffectBase effectBase);
        bool TryGetEffect(string effectType, out IEffectBase effectBase);
        bool RemoveEffect(IEffectBase effectBase);
        IEnumerator<IEffectBase> RemoveEffects(string effectType);
        IEnumerator<IEffectBase> RemoveAllEffects();
    }
}