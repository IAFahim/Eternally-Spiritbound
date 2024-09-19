using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Soul2.Modifiers.Runtime
{
    [Serializable]
    public class Modifier
    {
        [SerializeField] private float baseValue;
        [SerializeField] private float multiplier;
        [SerializeField] private float additive;

        public float Value => baseValue * multiplier + additive;

        public Modifier(float baseValue, float multiplier = 1f, float additive = 0f)
        {
            this.baseValue = baseValue;
            this.multiplier = multiplier;
            this.additive = additive;
        }

        public float BaseValue
        {
            get => baseValue;
            set => baseValue = value;
        }

        public float Multiplier
        {
            get => multiplier;
            set => multiplier = value;
        }

        public float Additive
        {
            get => additive;
            set => additive = value;
        }

        public void Set(float baseVal, float mul, float add)
        {
            baseValue = baseVal;
            multiplier = mul;
            additive = add;
        }

        /// <summary>
        ///     Applies a chance-based multiplier to the current Value.
        /// </summary>
        /// <param name="chance">
        ///     The probability of applying the bonus multiplier. If >= 1, it's treated as a guaranteed
        ///     multiplier.
        /// </param>
        /// <param name="bonusMultiplier">The multiplier to apply if the chance check succeeds.</param>
        /// <returns>The result of Value multiplied by the determined multiplier.</returns>
        public float ApplyChanceMultiplier(float chance, float bonusMultiplier)
        {
            var chanceMultiplier = chance >= 1f ? chance : Random.value < chance ? bonusMultiplier : 1f;
            return Value * chanceMultiplier;
        }

        public static implicit operator float(Modifier modifier)
        {
            return modifier.Value;
        }

        public static implicit operator Modifier(float value)
        {
            return new Modifier(value);
        }
    }
}