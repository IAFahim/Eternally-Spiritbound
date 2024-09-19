using System;

namespace Soul2.Conditions.Runtime
{
    [Serializable]
    public class Perform<T>
    {
        public bool active;
        public T value;
        public bool performing;


        public Perform(T value)
        {
            active = true;
            this.value = value;
        }

        public Perform(bool active, T value)
        {
            this.active = active;
            this.value = value;
        }

        public static implicit operator Perform<T>(T v)
        {
            return new Perform<T>(v);
        }

        public static implicit operator T(Perform<T> o)
        {
            return o.value;
        }

        public static implicit operator bool(Perform<T> o)
        {
            return o.active;
        }


        public override string ToString()
        {
            return "is " + (active ? "active" : "inactive") + " and " + (performing ? "performing" : "not performing") +
                   " " + value;
        }
    }
}