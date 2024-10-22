using _Root.Scripts.Game.FocusProvider.Runtime;
using Pancake;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableFocusConsumer : MonoBehaviour, IInteractable
    {
        public AssetReferenceGameObject confirmAsset;

        public Optional<SelectorDeSelectInteractComponent> confirmInstance;
        public Vector3 spawnOffset;

        public bool CanInteract(IInteractor initiator) => true;

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.IsFocused) ActiveConfirm();
        }

        public void OnInteractStart(FocusScriptable initiator)
        {
            initiator.Push(new(gameObject, false, OnInteractEnd));
        }
        

        public void OnInteractEnd(FocusScriptable focusScriptable)
        {
            HideConfirmInteract();
        }

        public void OnHoverExit(IInteractor initiator)
        {
            if (initiator.IsFocused) HideConfirmInteract();
        }

        private void HideConfirmInteract()
        {
            confirmInstance.Value.OnDeselectedWithoutClick();
        }


        private void ActiveConfirm()
        {
            if (!confirmInstance)
            {
                confirmInstance = ScriptablePool.Instance.Request(
                    confirmAsset,
                    transform.TransformPoint(spawnOffset),
                    Quaternion.identity,
                    transform
                ).GetComponent<SelectorDeSelectInteractComponent>();
            }
            
            confirmInstance.Value.Active();
        }


        private void OnDisable()
        {
            if (confirmInstance) ScriptablePool.Instance.Return(confirmAsset, confirmInstance.Value.gameObject);
        }
        
        public virtual void OnInteractEnd(IInteractor initiator)
        {
            // Do nothing
        }
        
        public virtual void OnInteractStart(IInteractor initiator)
        {
            // Do nothing
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