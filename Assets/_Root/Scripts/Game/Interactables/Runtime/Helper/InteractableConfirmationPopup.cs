using System;
using Pancake.Pools;
using Soul.Interactables.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime.Helper
{
    [RequireComponent(typeof(InteractableEntryPointComponent))]
    public class InteractableConfirmationPopup : MonoBehaviour
    {
        [SerializeField] private InteractableEntryPointComponent interactableEntryPointComponent;
        [SerializeField] private AssetReferenceGameObject confirmationPopupAsset;
        [SerializeField] protected Vector3[] offset = { new(0, 5, 0) };

        private IInteractableConfirmHelper _currentInteractableConfirmHelper;

        private void OnEnable()
        {
            interactableEntryPointComponent.OnEnteredEvent += OnEntered;
            interactableEntryPointComponent.OnExitEvent += OnExit;
        }

        private void OnEntered(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsFocused)
            {
                _currentInteractableConfirmHelper ??= SharedAssetReferencePool.Request(
                    confirmationPopupAsset,
                    transform.position,
                    Quaternion.identity
                ).GetComponent<IInteractableConfirmHelper>();
                _currentInteractableConfirmHelper.Init(interactableEntryPointComponent, offset);
            }
        }

        private void OnExit(IInteractorEntryPoint obj)
        {
            if (_currentInteractableConfirmHelper != null)
            {
                SharedAssetReferencePool.Return(confirmationPopupAsset, _currentInteractableConfirmHelper.GameObject);
                _currentInteractableConfirmHelper = null;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            foreach (var vector3 in offset)
            {
                Gizmos.DrawSphere(transform.position + vector3, 0.1f);
            }
        }
#endif
    }
}