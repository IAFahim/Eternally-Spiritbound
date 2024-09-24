using Pancake.Pools;

namespace Soul.PoolHelpers.Runtime
{
    public interface IInstantiateOrPool
    {
        public bool IsPoolable { get; }
        public AddressableGameObjectPool Pool { get; }
    }
}