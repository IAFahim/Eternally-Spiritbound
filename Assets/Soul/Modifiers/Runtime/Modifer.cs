using System;
using UnityEngine;

namespace Soul.Modifiers.Runtime
{
    [Serializable]
    public struct Modifier : IEquatable<Modifier>
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

        public bool MockRemaining(float subtract, out float remaining)
        {
            remaining = Value - subtract;
            if (remaining < 0)
            {
                remaining = Mathf.Abs(remaining);
                return true;
            }

            remaining = 0;
            return false;
        }


        public Modifier(float baseValue)
        {
            this.baseValue = baseValue;
            rate = 0;
            additive = 0;
            OnValueChanged = null;
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
            OnValueChanged = null;
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

        public void SetWithoutNotify(Modifier other)
        {
            baseValue = other.baseValue;
            rate = other.rate;
            additive = other.additive;
        }

        public void SetWithoutNotify(float baseVal, float mul, float add)
        {
            baseValue = baseVal;
            rate = mul;
            additive = add;
        }

        public void SetBaseWithoutNotify(float baseVal)
        {
            baseValue = baseVal;
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

        public bool Equals(Modifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return baseValue.Equals(other.baseValue) && rate.Equals(other.rate) &&
                   additive.Equals(other.additive);
        }

        public string ToString(string format) => Value.ToString(format);
        public override string ToString() => $"{baseValue} * {rate} + {additive} = {Value}";

        public void Deconstruct(out float baseValue, out float rate, out float additive)
        {
            baseValue = this.baseValue;
            rate = this.rate;
            additive = this.additive;
        }
    }
}