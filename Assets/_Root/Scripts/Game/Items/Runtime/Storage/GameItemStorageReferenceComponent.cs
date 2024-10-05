using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class GameItemStorageReferenceComponent : MonoBehaviour, IGameItemStorageReference
    {
        public GameItemStorage itemStorage;

        private void OnEnable()
        {
            itemStorage.InitializeStorage();
            foreach (var (key, value) in itemStorage) key.Initialize(gameObject, value);
        }

        public IStorageBase<GameItem, int> GameItemStorage => itemStorage;
    }
}