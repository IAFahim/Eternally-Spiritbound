using Soul2.Storages.Runtime;

namespace _Root.Scripts.Game.Storages
{
    public interface IStringStorageReference
    {
        public IStorageBase<string, int> Storage { get; }
    }
}