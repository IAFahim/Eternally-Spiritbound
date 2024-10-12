using _Root.Scripts.Game.Items.Runtime;
using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Storages.Runtime
{
    public class GameItemStorageReferenceComponent : MonoBehaviour, IGameItemStorageReference
    {
        public GameItemStorage itemStorage;

        private void OnEnable()
        {
            itemStorage.InitializeStorage();
            foreach (var (key, value) in itemStorage) key.OnAddedToInventory(gameObject, value);
        }

        public IStorageBase<GameItem, int> GameItemStorage => itemStorage;
        
    }
}