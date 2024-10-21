using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
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

        public void OnSelected(MainStackScriptable info)
        {
            Pass(info);
        }

        private void Pass(MainStackScriptable info)
        {
            var interactor = info.mainObject.GetComponent<IInteractor>();
            info.Push(transform.parent.gameObject, false);
            _interactableParent.OnInteractStart(interactor);
        }

        public void OnDeselected(RaycastHit lastHitInfo, MainStackScriptable info)
        {
            info.Pop();
        }

        public void OnReselected(MainStackScriptable info)
        {
            Pass(info);
        }
    }
}