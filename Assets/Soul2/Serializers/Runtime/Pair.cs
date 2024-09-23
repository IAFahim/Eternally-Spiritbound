using System;
using System.Collections.Generic;
using UnityEngine;

namespace Soul2.Serializers.Runtime
{
    [Serializable]
    public class Pair<T, TV>
    {
        [SerializeField] private T keyFirst;
        [SerializeField] private TV valueSecond;

        public Pair()
        {
        }

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

        public static implicit operator Pair<T, TV>((T, TV) tuple) => new() { Key = tuple.Item1, Value = tuple.Item2 };

        public static implicit operator (T, TV)(Pair<T, TV> pair) => (pair.keyFirst, pair.valueSecond);
        
        public static implicit operator Pair<T, TV>(KeyValuePair<T, TV> pair) => new() { Key = pair.Key, Value = pair.Value };
        
        public static implicit operator KeyValuePair<T, TV>(Pair<T, TV> pair) => new(pair.keyFirst, pair.valueSecond);
    }

    public static class PairExtensions
    {
        public static Pair<T, TV> ToPair<T, TV>(this (T, TV) tuple)
        {
            var pair = new Pair<T, TV>
            {
                Key = tuple.Item1,
                Value = tuple.Item2
            };
            return pair;
        }


        public static Pair<TKey, TValue>[] ToPairArray<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var pairs = new Pair<TKey, TValue>[dictionary.Count];
            int index = 0;
            foreach (var pair in dictionary)
            {
                pairs[index] = new Pair<TKey, TValue>
                {
                    Key = pair.Key,
                    Value = pair.Value
                };
                index++;
            }

            return pairs;
        }
    }
}