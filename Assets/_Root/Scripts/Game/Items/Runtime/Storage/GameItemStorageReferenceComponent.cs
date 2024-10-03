using _Root.Scripts.Game.Guid;
using Soul.Storages.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class GameItemStorageReferenceComponent : MonoBehaviour, IGameItemStorageReference
    {
        [FormerlySerializedAs("storage")] public GameItemStorage itemStorage;
        private void Awake()
        {
            TryGetComponent<ITitleGuidReference>(out var guidProvider);
            itemStorage.Guid = guidProvider.TitleGuid.guid;
        }

        public IStorageBase<GameItem, int> GameItemStorage => itemStorage;
    }
}