using System;
using System.Collections.Generic;
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

    public static class PairExtensions
    {
        public static Pair<T, TV> ToPair<T, TV>(this (T, TV) tuple)
        {
            return new Pair<T, TV>(tuple.Item1, tuple.Item2);
        }

        public static Pair<TKey, TValue>[] ToPairArray<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var pairs = new Pair<TKey, TValue>[dictionary.Count];
            int index = 0;
            foreach (var pair in dictionary)
            {
                pairs[index] = new Pair<TKey, TValue>(pair.Key, pair.Value);
                index++;
            }

            return pairs;
        }
    }
}