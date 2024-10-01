using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Soul.Modifiers.Runtime
{
    [Serializable]
    public class Modifier : IEquatable<Modifier>
    {
        [Newtonsoft.Json.JsonIgnore] [SerializeField]
        private float baseValue;

        [SerializeField] private float rate;
        [SerializeField] private float additive;

        public event Action<Modifier, float, float> OnValueChanged;

        /// <summary>
        /// Gets the calculated value of the modifier.
        /// </summary>
        public float Value => baseValue * (1 + rate) + additive;


        public Modifier(float baseValue)
        {
            this.baseValue = baseValue;
            this.rate = 0;
            this.additive = 0;
        }


        /// <summary>
        /// Creates a new Modifier instance.
        /// </summary>
        /// <param name="baseValue">The base value of the modifier.</param>
        /// <param name="rate">The rate to apply to the base value. Defaults to 1.</param>
        /// <param name="additive">The additive value to apply after multiplication. Defaults to 0.</param>
        public Modifier(float baseValue, float rate, float additive = 0f)
        {
            this.baseValue = baseValue;
            this.rate = rate;
            this.additive = additive;
        }

        public float BaseValue
        {
            get => baseValue;
            set
            {
                if (Mathf.Approximately(baseValue, value)) return;
                float oldValue = Value;
                baseValue = value;
                OnValueChanged?.Invoke(this, oldValue, Value);
            }
        }

        public float Rate
        {
            get => rate;
            set
            {
                if (Mathf.Approximately(rate, value)) return;
                float oldValue = Value;
                rate = value;
                OnValueChanged?.Invoke(this, oldValue, Value);
            }
        }

        public float Additive
        {
            get => additive;
            set
            {
                if (Mathf.Approximately(additive, value)) return;
                var oldValue = Value;
                additive = value;
                OnValueChanged?.Invoke(this, oldValue, Value);
            }
        }

        /// <summary>
        /// Sets all components of the modifier at once.
        /// </summary>
        public void Set(float baseVal, float mul, float add)
        {
            float oldValue = Value;
            baseValue = baseVal;
            rate = mul;
            additive = add;
            OnValueChanged?.Invoke(this, oldValue, Value);
        }

        public void SetBaseWithoutNotify(float baseVal)
        {
            baseValue = baseVal;
        }

        /// <summary>
        /// Applies a chance-based rate to the current Value.
        /// </summary>
        /// <param name="chance">The probability of applying the bonus rate. If >= 1, it's treated as a guaranteed rate.</param>
        /// <param name="bonusMultiplier">The rate to apply if the chance check succeeds.</param>
        /// <returns>The result of Value multiplied by the determined rate.</returns>
        public float ApplyChanceMultiplier(float chance, float bonusMultiplier)
        {
            float chanceMultiplier = chance >= 1f ? chance : Random.value < chance ? bonusMultiplier : 1f;
            return Value * chanceMultiplier;
        }

        /// <summary>
        /// Creates a deep copy of this Modifier.
        /// </summary>
        public Modifier Clone() => new Modifier(baseValue, rate, additive);

        /// <summary>
        /// Combines this Modifier with another Modifier.
        /// </summary>
        public Modifier Combine(Modifier other)
        {
            return new Modifier(
                baseValue * other.baseValue,
                rate * other.rate,
                additive + other.additive
            );
        }

        public static implicit operator float(Modifier modifier) => modifier.Value;

        public static implicit operator Modifier(float value) => new Modifier(value);

        public bool Equals(Modifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return baseValue.Equals(other.baseValue) && rate.Equals(other.rate) &&
                   additive.Equals(other.additive);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Modifier)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = baseValue.GetHashCode();
                hashCode = (hashCode * 397) ^ rate.GetHashCode();
                hashCode = (hashCode * 397) ^ additive.GetHashCode();
                return hashCode;
            }
        }

        public string ToString(string format) => Value.ToString(format);
        public override string ToString() => $"{baseValue} * {rate} + {additive} = {Value}";
    }
}