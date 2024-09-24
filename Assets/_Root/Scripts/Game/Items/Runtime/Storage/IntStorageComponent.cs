using _Root.Scripts.Game.Guid;
using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class IntStorageComponent : MonoBehaviour, IIntStorageReference<ItemBase>
    {
        public ItemStorage storage;

        private void Awake()
        {
            TryGetComponent<IGuidProvider>(out var guidProvider);
            storage.Guid = guidProvider.Guid;
        }

        public IStorageBase<ItemBase, int> Storage => storage;
    }
}