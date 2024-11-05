using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Relationships.Runtime
{
    public class Direct<T, TV> : ScriptableObject
    {
        public UnityDictionary<T, TV> edges;

        public TV this[T key]
        {
            get => edges[key];
            set => edges[key] = value;
        }
    }
}