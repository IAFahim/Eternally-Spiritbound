using Soul.Storages.Runtime;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public interface IItemStorage
    {
        IStorageBase<GameItem, int> Storage { get; }
    }
}