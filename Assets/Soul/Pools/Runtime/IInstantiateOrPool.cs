using Pancake.Pools;

namespace Soul.Pools.Runtime
{
    public interface IInstantiateOrPool
    {
        public bool IsPoolable { get; }
        public AddressableGameObjectPool Pool { get; }
    }
}