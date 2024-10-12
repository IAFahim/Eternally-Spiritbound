using System;
using System.Collections.Generic;
using Soul.Effects.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Effects.Runtime
{
    [Serializable]
    public class EffectConsumer : IEffectConsumerBase
    {
        public List<EffectInstance> effects;
        public int ActiveEffectCount => effects.Count;
        public EffectInfluencesScriptable effectInfluences;

        public EffectConsumer(List<EffectInstance> effects)
        {
            this.effects = effects;
        }


        public int EffectCount(string effectType) =>
            effects.FindAll(effect => effect.BaseEffect.EffectType == effectType).Count;

        public float StatMultiplier
        {
            get => effectInfluences.influenceMultiplier;
            set => effectInfluences.influenceMultiplier = value;
        }

        public IEnumerator<EffectInstance> GetEffects(string effectType)
        {
            foreach (var effect in effects)
            {
                if (effect.BaseEffect.EffectType == effectType) yield return effect;
            }
        }

        public void Update()
        {
            for (var i = 0; i < effects.Count; i++)
            {
                if (!effects[i].KeepUpdating(Time.deltaTime))
                {
                    effects.RemoveAt(i);
                    i--;
                }
            }
        }

        public float GetEffectMultiplier(string effectType)
        {
            return effectInfluences.GetInfluence(effectType) * StatMultiplier;
        }

        public bool CanApplyEffect(string effectType, out float effectStrength)
        {
            effectStrength = GetEffectMultiplier(effectType);
            return effectStrength > 0;
        }

        public void Apply(EffectInstance effectBase) => effects.Add(effectBase);

        public bool TryGetEffect(string effectType, out EffectInstance effectBase)
        {
            effectBase = effects.Find(effect => effect.BaseEffect.EffectType == effectType);
            return effectBase != null;
        }

        public bool RemoveEffect(EffectInstance effectBase)
        {
            return effects.Remove(effectBase);
        }

        public IEnumerator<EffectInstance> RemoveEffects(string effectType)
        {
            for (var i = 0; i < effects.Count; i++)
            {
                if (effects[i].BaseEffect.EffectType == effectType)
                {
                    yield return effects[i];
                    effects.RemoveAt(i);
                    i--;
                }
            }
        }

        public IEnumerator<EffectInstance> RemoveAllEffects()
        {
            foreach (var effect in effects)
            {
                yield return effect;
            }

            effects.Clear();
        }
    }
}