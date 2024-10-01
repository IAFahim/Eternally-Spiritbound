using _Root.Scripts.Game.Guid;
using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class IntStorageComponent : MonoBehaviour, IItemStorage
    {
        public ItemStorage storage;

        private void Awake()
        {
            TryGetComponent<ITitleGuidReference>(out var guidProvider);
            storage.Guid = guidProvider.TitleGuid.guid;
        }

        public IStorageBase<GameItem, int> Storage => storage;
    }
}