using System;
using UnityEngine;

namespace Soul2.Conditions.Runtime
{
    [Serializable]
    public class Performing<T>
    {
        [SerializeField] private bool active;
        [SerializeField] private T value;

        public bool Active
        {
            get => active;
            set => active = value;
        }

        public T Value => value;

        public Performing(T value)
        {
            active = true;
            this.value = value;
        }

        public Performing(bool active, T value)
        {
            this.active = active;
            this.value = value;
        }

        public static implicit operator Performing<T>(T v)
        {
            return new Performing<T>(v);
        }

        public static implicit operator T(Performing<T> o)
        {
            return o.Value;
        }

        public static implicit operator bool(Performing<T> o)
        {
            return o.active;
        }

        public static bool operator ==(Performing<T> lhs, Performing<T> rhs)
        {
            if (lhs != null && lhs.Value is null) return rhs != null && rhs.Value is null;
            return rhs != null && lhs != null && lhs.Value.Equals(rhs.Value);
        }

        public static bool operator !=(Performing<T> lhs, Performing<T> rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (Value is null) return obj is null;
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}