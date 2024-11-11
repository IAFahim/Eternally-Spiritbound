using Pancake;
using Soul.Serializers.Runtime;

namespace Soul.Relationships.Runtime
{
    public abstract class Graph<T, TV> : StringConstant
    {
        [Guid] public string guid;
        public UnityDictionary<T, TV> edges;
        
        public abstract T GetSource(string source);

        public TV this[string source]
        {
            get => edges[GetSource(source)];
            set => edges[GetSource(source)] = value;
        }

        public TV this[T source]
        {
            get => edges[source];
            set => edges[source] = value;
        }
    }
}