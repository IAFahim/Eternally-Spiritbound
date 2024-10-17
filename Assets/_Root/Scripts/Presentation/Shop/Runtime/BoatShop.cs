using System;
using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    public class BoatShop : InteractableComponent, ISelectionCallback
    {
        public Weapon[] weapons;
        public MainObjectProviderScriptable mainObjectProviderScriptable;
        public AssetReferenceGameObject interactableSignPrefab;

        public override bool CanInteract(GameObject initiator) => true;

        public override void OnInteractHoverEnter(GameObject initiator)
        {
            Debug.Log("Hovering");
            if (initiator == mainObjectProviderScriptable.mainGameObjectInstance)
            {
                Addressables.InstantiateAsync(interactableSignPrefab).Completed += OnInteractSignSpawnComplete;
            }
        }

        private void OnInteractSignSpawnComplete(AsyncOperationHandle<GameObject> handle)
        {
            var interactableSign = handle.Result;
            interactableSign.transform.position = transform.position;
        }


        public override void OnInteractStart(GameObject initiator, Action onComplete)
        {
        }

        public override void OnHoverExit(GameObject initiator)
        {
            Debug.Log("Hover Exit");
        }

        public void OnSelected(RaycastHit hit)
        {
            Debug.Log("Selected");
        }

        public void OnDeselected(RaycastHit hit)
        {
            Debug.Log("Deselected");
        }

        public void OnReselected(RaycastHit hit)
        {
            Debug.Log("Reselected");
        }
    }
}