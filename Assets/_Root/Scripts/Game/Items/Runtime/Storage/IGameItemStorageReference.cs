using Soul.Storages.Runtime;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public interface IGameItemStorageReference
    {
        IStorageBase<GameItem, int> GameItemStorage { get; }
    }
}