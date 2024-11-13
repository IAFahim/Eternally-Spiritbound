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
        [SerializeField] private string source;
        [SerializeField] private List<Pair<T, TV>> dictionary;
        public readonly Dictionary<T, TV> Dictionary = new();
        public virtual bool TryGetValue(string key, out TV value) => Dictionary.TryGetValue(GetSource(key), out value);
        public virtual bool TryGetValue(T key, out TV value) => Dictionary.TryGetValue(key, out value);
        public abstract T GetSource(string source);

        private void Initialize()
        {
            Dictionary.Clear();
            foreach (var (key, value) in dictionary) Dictionary.Add(key, value);
        }
        
        protected virtual void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            Initialize();
        }

        protected virtual void OnValidate() => Initialize();
    }
}