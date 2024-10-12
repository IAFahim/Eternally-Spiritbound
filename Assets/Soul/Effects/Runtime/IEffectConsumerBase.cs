using System.Collections.Generic;

namespace Soul.Effects.Runtime
{
    public interface IEffectConsumerBase
    {
        int ActiveEffectCount { get; }
        int EffectCount(string effectType);
        public float StatMultiplier { get; set; }
        float GetEffectMultiplier(string effectType);
        bool CanApplyEffect(string effectType, out float effectStrength);
        IEnumerator<EffectInstance> GetEffects(string effectType);
        void Apply(EffectInstance effectBase);
        bool TryGetEffect(string effectType, out EffectInstance effectBase);
        bool RemoveEffect(EffectInstance effectBase);
        IEnumerator<EffectInstance> RemoveEffects(string effectType);
        IEnumerator<EffectInstance> RemoveAllEffects();
    }
}