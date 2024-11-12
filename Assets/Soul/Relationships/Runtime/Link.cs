using System;
using System.Collections.Generic;
using Pancake;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Relationships.Runtime
{
    public abstract class Link<T, TV> : ScriptableObject
    {
        [Guid] public string guid;
        [SerializeField] private List<Pair<T, TV>> dictionary;
        public readonly Dictionary<T, TV> Dictionary = new();

        public abstract T GetSource(string source);

        public TV this[string key]
        {
            get => Dictionary[GetSource(key)];
            set => Dictionary[GetSource(key)] = value;
        }

        public TV this[T key]
        {
            get => Dictionary[key];
            set => Dictionary[key] = value;
        }

        protected virtual void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            Initialize();
        }

        protected virtual void OnValidate() => Initialize();

        private void Initialize()
        {
            Dictionary.Clear();
            foreach (var item in dictionary) Dictionary.Add(item, default);
        }
    }
}