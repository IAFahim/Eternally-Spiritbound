using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Serializers.Runtime
{
    [Serializable]
    public class Pair<T, TV>
    {
        [SerializeField] private T key;
        [SerializeField] private TV value;

        public Pair()
        {
        }

        public Pair(T key, TV value)
        {
            this.key = key;
            this.value = value;
        }

        public Pair(Pair<T, TV> source)
        {
            key = source.key;
            value = source.value;
        }

        public T Key
        {
            get => key;
            set => key = value;
        }

        public T First
        {
            get => key;
            set => key = value;
        }

        public TV Value
        {
            get => value;
            set => this.value = value;
        }

        public TV Multiplier
        {
            get => value;
            set => this.value = value;
        }

        public TV Second
        {
            get => value;
            set => this.value = value;
        }


        private string GetIdentity()
        {
            return "[" + key + " , " + value + "]";
        }

        public override string ToString()
        {
            return GetIdentity();
        }


        public static implicit operator T(Pair<T, TV> pair)
        {
            return pair.key;
        }

        public static implicit operator TV(Pair<T, TV> pair)
        {
            return pair.value;
        }

        public static implicit operator Pair<T, TV>((T, TV) tuple) => new() { Key = tuple.Item1, Value = tuple.Item2 };

        public static implicit operator (T, TV)(Pair<T, TV> pair) => (pair.key, pair.value);

        public static implicit operator Pair<T, TV>(KeyValuePair<T, TV> pair) =>
            new() { Key = pair.Key, Value = pair.Value };

        public static implicit operator KeyValuePair<T, TV>(Pair<T, TV> pair) => new(pair.key, pair.value);


        public void Deconstruct(out T first, out TV second)
        {
            first = this.key;
            second = this.value;
        }
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