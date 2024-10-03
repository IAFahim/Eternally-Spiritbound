using _Root.Scripts.Game.Guid;
using Soul.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class GameItemStorageReferenceComponent : MonoBehaviour, IGameItemStorageReference
    {
        public bool load;
        public GameItemStorage itemStorage;
        private void Awake()
        {
            TryGetComponent<ITitleGuidReference>(out var guidProvider);
            itemStorage.InitializeStorage(guidProvider.TitleGuid.guid, load);
        }

        private void OnEnable()
        {
            foreach (var (key, _) in itemStorage) key.Initialize(gameObject);
        }

        public IStorageBase<GameItem, int> GameItemStorage => itemStorage;
    }
}