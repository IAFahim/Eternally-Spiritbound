using _Root.Scripts.Game.FocusProvider.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    [SelectionBase]
    public class InteractableFocusConsumer : MonoBehaviour, IInteractable
    {
        [FormerlySerializedAs("interactableSignPrefab")]
        public AssetReferenceGameObject confirmAsset;

        [FormerlySerializedAs("signInstance")] public Optional<InteractConformer> confirmInstance;
        public Vector3 spawnOffset;

        public bool CanInteract(IInteractor initiator) => true;

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.IsFocused) ActiveConfirm();
        }


        public virtual void OnInteractStart(IInteractor initiator)
        {
            if (initiator.IsFocused)
            {
                DeactivateConfirm();
                FocusScriptable.Instance.Push(gameObject, true);
            }
        }

        public virtual void OnInteractEnd(IInteractor initiator)
        {
            if (initiator.IsFocused)
            {
                ActiveConfirm();
                FocusScriptable.Instance.Pop();
            }
        }

        public void OnHoverExit(IInteractor initiator)
        {
            if (initiator.IsFocused) DeactivateConfirm();
        }

        private void DeactivateConfirm()
        {
            if (confirmInstance) confirmInstance.Value.gameObject.SetActive(false);
        }

        private void ActiveConfirm()
        {
            if (!confirmInstance)
            {
                Addressables.InstantiateAsync(confirmAsset, transform).Completed += OnConfirmInstantiateComplete;
            }
            else confirmInstance.Value.gameObject.SetActive(true);
        }

        private void OnConfirmInstantiateComplete(AsyncOperationHandle<GameObject> handle)
        {
            confirmInstance = handle.Result.GetComponent<InteractConformer>();
            confirmInstance.Value.transform.position = transform.TransformPoint(spawnOffset);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(spawnOffset), .5f);
        }

#endif
    }
}