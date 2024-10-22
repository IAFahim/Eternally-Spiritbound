using _Root.Scripts.Game.FocusProvider.Runtime;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class FocusConformer : MonoBehaviour, IInteractable
    {
        [SerializeField] private AssetReferenceGameObject confirmAsset;
        [SerializeField] private bool isMain;

        [SerializeField] private SelectorDeSelectInteractComponent confirmInstance;
        [SerializeField] private Vector3 spawnOffset;

        public bool hasFocus;
        public virtual bool CanInteract(IInteractor initiator) => true;

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.IsFocused) ActiveConfirm();
        }

        public void OnInteractStart(FocusScriptable initiator)
        {
            hasFocus = true;
            initiator.Push(new FocusInfo(gameObject, isMain, OnInteractEnd));
        }
        
        public void OnInteractEnd(FocusScriptable focusScriptable)
        {
            HideConfirmInteract();
            hasFocus = false;
        }

        public void OnHoverExit(IInteractor initiator)
        {
            if (initiator.GameObject == FocusScriptable.Instance.mainObject)
            {
                if (hasFocus) FocusScriptable.Instance.TryPopAndActiveLast();
                ScriptablePool.Instance.Return(confirmAsset, confirmInstance.gameObject);
                confirmInstance = null;
            }
        }

        private void HideConfirmInteract()
        {
            confirmInstance.OnDeselectedWithoutClick();
        }


        private void ActiveConfirm()
        {
            if (!confirmInstance)
            {
                confirmInstance = ScriptablePool.Instance.Request(
                    confirmAsset,
                    transform.TransformPoint(spawnOffset),
                    Quaternion.identity
                ).GetComponent<SelectorDeSelectInteractComponent>();
            }

            confirmInstance.Active(this);
        }


        private void OnDisable()
        {
            if (confirmInstance) ScriptablePool.Instance.Return(confirmAsset, confirmInstance.gameObject);
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