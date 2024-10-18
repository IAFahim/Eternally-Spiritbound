using System;
using System.Collections.Generic;
using _Root.Scripts.Game.GameEntities.Runtime.Weapons;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Presentation.FocusProvider.Runtime;
using _Root.Scripts.Presentation.Selectors.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Presentation.Shop.Runtime
{
    [SelectionBase]
    public class BoatShop : InteractableComponent, IFocusProvider
    {
        public Weapon[] weapons;
        public BoatShopFocusProviderScriptable boatShopFocusProviderScriptable;
        public AssetReferenceGameObject interactableSignPrefab;
        public Optional<InteractPasser> signInstance;
        public Vector3 spawnOffset;

        public override bool CanInteract(IInteractor initiator) => true;

        public override void OnInteractHoverEnter(IInteractor initiator)
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


        public override void OnInteractStart(IInteractor initiator)
        {
            
        }

        public override void OnInteractEnd(IInteractor initiator)
        {
            
        }

        public override void OnHoverExit(IInteractor initiator)
        {
            if (signInstance) signInstance.Value.gameObject.SetActive(false);
            Debug.Log("Hover Exit");
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