using System;
using Pancake;
using Soul.Interactables.Runtime;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableComponent : MonoBehaviour, IInteractable
    {
        public Optional<AssetReferenceGameObject> interactableConfirmHelperAsset;
        [SerializeField] private Vector3 offset = new(0, 5, 0);
        private IInteractorEntryPoint _interactorEntryPoint;
        private IInteractableConfirmHelper _currentInteractableConfirmHelper;

        public Transform Transform => transform;

        private void Awake() => _interactorEntryPoint = GetComponent<IInteractorEntryPoint>();

        public virtual void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactableConfirmHelperAsset.Enabled && interactorEntryPoint.IsFocused)
            {
                _currentInteractableConfirmHelper ??= interactableConfirmHelperAsset.Value
                    .Request(transform.TransformPoint(offset), Quaternion.identity)
                    .GetComponent<IInteractableConfirmHelper>();

                _currentInteractableConfirmHelper.Active(this);
            }
        }


        public virtual void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactableConfirmHelperAsset.Enabled) _currentInteractableConfirmHelper?.Hide();
        }

        public virtual void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsFocused) _interactorEntryPoint.IsFocused = true;
        }

        public virtual void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsFocused) _interactorEntryPoint.IsFocused = false;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (interactableConfirmHelperAsset.Enabled)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.TransformPoint(offset), 0.1f);
            }
        }
#endif
    }
}