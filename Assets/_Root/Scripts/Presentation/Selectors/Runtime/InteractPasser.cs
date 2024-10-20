using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;

namespace _Root.Scripts.Presentation.Selectors.Runtime
{
    public class InteractPasser : MonoBehaviour, ISelector
    {
        private IInteractable _interactableParent;

        private void Awake()
        {
            _interactableParent = GetComponentInParent<IInteractable>();
        }

        public void OnSelected(MainProviderScriptable info)
        {
            Pass(info);
        }

        private void Pass(MainProviderScriptable info)
        {
            var interactor = info.mainGameObjectInstance.GetComponent<IInteractor>();
            info.ProvideTo(transform.parent.gameObject);
            _interactableParent.OnInteractStart(interactor);
        }

        public void OnDeselected(RaycastHit lastHitInfo, MainProviderScriptable info)
        {
            info.ReturnToPreviousObject();
        }

        public void OnReselected(MainProviderScriptable info)
        {
            Pass(info);
        }
    }
}