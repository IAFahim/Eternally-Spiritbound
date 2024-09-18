using Pancake.Pools;

namespace Soul2.PoolHelpers.Runtime
{
    public interface IInstantiateOrPool
    {
        public bool IsPoolable { get; }
        public AddressableGameObjectPool Pool { get; }
    }
}