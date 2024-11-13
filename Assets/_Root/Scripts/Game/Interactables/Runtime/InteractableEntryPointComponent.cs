using System;
using Pancake;
using Pancake.Pools;
using Soul.Interactables.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableEntryPointComponent : MonoBehaviour, IInteractableEntryPoint
    {
        public Optional<AssetReferenceGameObject> interactableConfirmHelperAsset;
        [SerializeField] private Vector3 offset = new(0, 5, 0);
        private IInteractorEntryPoint _interactorEntryPoint;
        private IInteractableConfirmHelper _currentInteractableConfirmHelper;
        public Transform Transform => transform;

        public event Action<IInteractorEntryPoint> OnInteractionStartedEvent;
        public event Action<IInteractorEntryPoint> OnInteractionEndedEvent;

        private void Awake() => _interactorEntryPoint = GetComponent<IInteractorEntryPoint>();

        public virtual void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint)
        {
            OnInteractionStartedEvent?.Invoke(interactorEntryPoint);
            if (interactorEntryPoint.IsFocused) _interactorEntryPoint.IsFocused = true;
        }

        public virtual void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint)
        {
            OnInteractionEndedEvent?.Invoke(interactorEntryPoint);
            if (interactorEntryPoint.IsFocused) _interactorEntryPoint.IsFocused = false;
        }

        public virtual void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactableConfirmHelperAsset.Enabled && interactorEntryPoint.IsFocused)
            {
                _currentInteractableConfirmHelper ??= SharedAssetReferencePool.Request(
                    interactableConfirmHelperAsset.Value,
                    transform.TransformPoint(offset),
                    Quaternion.identity
                ).GetComponent<IInteractableConfirmHelper>();

                _currentInteractableConfirmHelper.Active(this);
            }
        }


        public virtual void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsMain) _currentInteractableConfirmHelper?.Hide();
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