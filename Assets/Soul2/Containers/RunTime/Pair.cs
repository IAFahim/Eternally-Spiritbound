using System;
using UnityEngine;

namespace Soul2.Containers.RunTime
{
    [Serializable]
    public class Pair<T, TV>
    {
        [SerializeField] private T keyFirst;
        [SerializeField] private TV valueSecond;

        public Pair(T keyFirst, TV valueSecond)
        {
            this.keyFirst = keyFirst;
            this.valueSecond = valueSecond;
        }

        public Pair(Pair<T, TV> source)
        {
            keyFirst = source.keyFirst;
            valueSecond = source.valueSecond;
        }

        public T Key
        {
            get => keyFirst;
            set => keyFirst = value;
        }

        public T First
        {
            get => keyFirst;
            set => keyFirst = value;
        }

        public TV Value
        {
            get => valueSecond;
            set => valueSecond = value;
        }

        public TV Multiplier
        {
            get => valueSecond;
            set => valueSecond = value;
        }

        public TV Second
        {
            get => valueSecond;
            set => valueSecond = value;
        }


        private string GetIdentity()
        {
            return "[" + keyFirst + " , " + valueSecond + "]";
        }

        public override string ToString()
        {
            return GetIdentity();
        }
        

        public static implicit operator T(Pair<T, TV> pair)
        {
            return pair.keyFirst;
        }

        public static implicit operator TV(Pair<T, TV> pair)
        {
            return pair.valueSecond;
        }
    }
}