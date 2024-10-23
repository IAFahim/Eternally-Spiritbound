using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Selectors.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableConfirmHelperComponent : MonoBehaviour, ISelector, IInteractableConfirmHelper
    {
        [SerializeField] private bool endInteractOnDeselect;
        [SerializeField] private UnityEvent activeEvent;
        [SerializeField] private UnityEvent selectedEvent;
        [SerializeField] private UnityEvent deselectedEvent;

        private IInteractable _interactableParent;
        public GameObject GameObject => gameObject;

        public void Active(IInteractable interactable)
        {
            _interactableParent = interactable;
            activeEvent.Invoke();
        }

        public void OnSelected(FocusScriptable info)
        {
            Pass(info);
        }

        public void OnDeselected(RaycastHit lastHitInfo, FocusScriptable info)
        {
            if (endInteractOnDeselect)
            {
                _interactableParent.OnInteractEnd(info, info.mainObject.GetComponent<IInteractor>());
                FocusScriptable.Instance.TryPopAndActiveLast();
                Hide();
            }
        }

        public void OnReselected(FocusScriptable focusScriptable)
        {
            Pass(focusScriptable);
        }

        public void Hide()
        {
            deselectedEvent.Invoke();
        }

        private void Pass(FocusScriptable info)
        {
            _interactableParent.OnInteractStart(info, info.mainObject.GetComponent<IInteractor>());
            selectedEvent.Invoke();
        }
    }
}