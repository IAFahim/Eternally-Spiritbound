using System;
using System.Collections.Generic;
using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Presentation.FocusProvider.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    [SelectionBase]
    public class BoatShop : InteractableComponent, ISelectionCallback, IFocusProvider
    {
        public Weapon[] weapons;
        public MainObjectProviderScriptable mainObjectProviderScriptable;
        public BoatShopFocusProviderScriptable boatShopFocusProviderScriptable;
        public AssetReferenceGameObject interactableSignPrefab;
        public Vector3 spawnOffset;

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
            interactableSign.transform.position = transform.TransformPoint(spawnOffset);
        }


        public override void OnInteractStart(GameObject initiator, Action onComplete)
        {
            mainObjectProviderScriptable.ProvideTo(gameObject);
        }

        public override void OnHoverExit(GameObject initiator)
        {
            if (initiator == mainObjectProviderScriptable.lastFocusedGameObject)
            {
                mainObjectProviderScriptable.ReturnControlToLastGameObject();
                return;
            }
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(spawnOffset), .5f);
        }

        public void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences, GameObject targetGameObject,
            Action returnFocusCallBack)
        {
            boatShopFocusProviderScriptable.SetFocus(activeElements, transformReferences, targetGameObject,
                returnFocusCallBack);
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            boatShopFocusProviderScriptable.OnFocusLost(targetGameObject);
        }
    }
}