using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using _Root.Scripts.Presentation.Selectors.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    [SelectionBase]
    public class BoatShop : FocusConsumerComponent, IInteractable
    {
        public Weapon[] weapons;

        public AssetReferenceGameObject interactableSignPrefab;
        public Optional<InteractPasser> signInstance;
        public Vector3 spawnOffset;

        public bool CanInteract(IInteractor initiator) => true;

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (!signInstance)
            {
                Addressables.InstantiateAsync(interactableSignPrefab, transform).Completed +=
                    OnInteractSignSpawnComplete;
            }
            else signInstance.Value.gameObject.SetActive(true);
        }

        private void OnInteractSignSpawnComplete(AsyncOperationHandle<GameObject> handle)
        {
            signInstance = handle.Result.GetComponent<InteractPasser>();
            signInstance.Value.transform.position = transform.TransformPoint(spawnOffset);
        }


        public void OnInteractStart(IInteractor initiator)
        {
        }

        public void OnInteractEnd(IInteractor initiator)
        {
        }

        public void OnHoverExit(IInteractor initiator)
        {
            if (signInstance) signInstance.Value.gameObject.SetActive(false);
            Debug.Log("Hover Exit");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(spawnOffset), .5f);
        }
    }
}