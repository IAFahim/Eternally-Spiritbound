using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Selectors.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractConformer : MonoBehaviour, ISelector
    {
        private IInteractable _interactableParent;

        private void Awake()
        {
            _interactableParent = GetComponentInParent<IInteractable>();
        }

        public void OnSelected(FocusScriptable info)
        {
            Pass(info);
        }

        private void Pass(FocusScriptable info)
        {
            var interactor = info.mainObject.GetComponent<IInteractor>();
            info.Push(transform.parent.gameObject, false);
            _interactableParent.OnInteractStart(interactor);
        }

        public void OnDeselected(RaycastHit lastHitInfo, FocusScriptable info)
        {
            info.Pop();
        }

        public void OnReselected(FocusScriptable info)
        {
            Pass(info);
        }
    }
}