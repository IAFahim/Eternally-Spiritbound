using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class IntStorageComponent : MonoBehaviour, IIntStorageReference<ItemBase>
    {
        public ItemStorage storage;
        private string _guid;
        private void Awake()
        {
            // TryGetComponent(I)
            
        }

        public IStorageBase<ItemBase, int> Storage => storage;
    }
}