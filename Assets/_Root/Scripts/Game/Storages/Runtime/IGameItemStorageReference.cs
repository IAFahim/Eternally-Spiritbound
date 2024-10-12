using _Root.Scripts.Game.Items.Runtime;
using Soul.Storages.Runtime;

namespace _Root.Scripts.Game.Storages.Runtime
{
    public interface IGameItemStorageReference
    {
        IStorageBase<GameItem, int> GameItemStorage { get; }
    }
}