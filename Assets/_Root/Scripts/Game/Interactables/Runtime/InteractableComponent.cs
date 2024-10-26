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
        private IInteractableConfirmHelper _interactableConfirmHelper;
        [SerializeField] private FocusEntryPointComponent focusEntryPointComponent;

        public Transform Transform => transform;

        public virtual void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint)
        {
            Debug.Log("Detected");
            if (interactableConfirmHelperAsset.Enabled && interactorEntryPoint.IsFocused)
            {
                _interactableConfirmHelper ??= interactableConfirmHelperAsset.Value
                    .Request(transform.TransformPoint(offset), Quaternion.identity)
                    .GetComponent<IInteractableConfirmHelper>();

                _interactableConfirmHelper.Active(this);
            }
        }


        public virtual void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactableConfirmHelperAsset.Enabled) _interactableConfirmHelper?.Hide();
        }

        public virtual void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsFocused) focusEntryPointComponent.IsFocused = true;
        }

        public virtual void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint)
        {
            if (interactorEntryPoint.IsFocused) focusEntryPointComponent.IsFocused = false;
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