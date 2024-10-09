using _Root.Scripts.Game.Combats.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Items.Runtime.Storage;
using UnityEngine;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Shop/GunShop")]
    public class GunShop : InteractableScriptable
    {
        public Weapon[] weapons;

        public override void OnInteractStart(GameObject initiator)
        {
            if (initiator.TryGetComponent<IGameItemStorageReference>(out var storageReference))
            {
                Debug.Log(storageReference.GameItemStorage.Count);
            }
        }

        public override void OnInteractEnd(GameObject initiator)
        {
            Debug.Log("End");
        }
    }
}