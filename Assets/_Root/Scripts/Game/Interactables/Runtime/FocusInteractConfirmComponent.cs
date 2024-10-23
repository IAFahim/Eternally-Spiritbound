using _Root.Scripts.Game.FocusProvider.Runtime;
using Pancake.Common;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class FocusInteractConfirmComponent : MonoBehaviour, IInteractable
    {
        [SerializeField] private AssetReferenceGameObject confirmAsset;
        [SerializeField] private bool isMain;

        private IInteractableConfirmHelper _interactableConfirmHelper;
        [SerializeField] private Vector3 spawnOffset;

        public bool hasFocus;
        public GameObject GameObject => gameObject;
        public virtual bool CanInteract(IInteractor initiator) => true;

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.IsFocused) ActiveConfirm();
        }

        public void OnInteractStart(FocusScriptable focusScriptable, IInteractor interactor)
        {
            hasFocus = true;
            interactor.OnInteractStart(this);
            focusScriptable.Push(new FocusInfo(gameObject, isMain, OnInteractEnd));
        }

        private void OnInteractEnd(FocusScriptable obj)
        {
            HideConfirmInteract();
            hasFocus = false;
        }

        public void OnInteractEnd(FocusScriptable focusScriptable, IInteractor interactor)
        {
        }


        public void OnHoverExit(IInteractor initiator)
        {
            if (initiator.GameObject == FocusScriptable.Instance.mainObject)
            {
                if (hasFocus) FocusScriptable.Instance.TryPopAndActiveLast();
                ScriptablePool.Instance.Return(confirmAsset, _interactableConfirmHelper.GameObject);
                _interactableConfirmHelper = null;
            }
        }

        private void HideConfirmInteract()
        {
            _interactableConfirmHelper.Hide();
        }


        private void ActiveConfirm()
        {
            _interactableConfirmHelper ??= ScriptablePool.Instance.Request(
                confirmAsset,
                transform.TransformPoint(spawnOffset),
                Quaternion.identity
            ).GetComponent<IInteractableConfirmHelper>();

            _interactableConfirmHelper.Active(this);
        }


        private void OnDisable()
        {
            if (_interactableConfirmHelper == null || _interactableConfirmHelper.GameObject.OrNull()) return;
            ScriptablePool.Instance.Return(confirmAsset, _interactableConfirmHelper.GameObject);
        }

        public virtual void OnInteractEnd(IInteractor initiator)
        {
            initiator.OnInteractEnd(this);
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