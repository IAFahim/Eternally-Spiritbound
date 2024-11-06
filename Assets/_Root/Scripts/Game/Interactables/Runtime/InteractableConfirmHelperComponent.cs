using Soul.Interactables.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableConfirmHelperComponent : MonoBehaviour, ISelector, IInteractableConfirmHelper
    {
        [SerializeField] private bool endInteractOnDeselect;
        [SerializeField] private FocusManagerScript focusManagerScript;
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

        public void OnSelected(RaycastHit hit)
        {
            InteractStart();
        }

        public void OnDeselected(RaycastHit lastHitInfo, RaycastHit hit)
        {
            if (endInteractOnDeselect) InteractEnd();
        }

        public void OnReselected(RaycastHit hit)
        {
            InteractStart();
        }

        public void Hide()
        {
            deselectedEvent.Invoke();
        }

        private void InteractStart()
        {
            selectedEvent.Invoke();
            _interactableParent.OnInteractionStarted(
                focusManagerScript.mainObject.GetComponent<IInteractorEntryPoint>()
            );
        }

        private void InteractEnd()
        {
            _interactableParent.OnInteractionEnded(focusManagerScript.mainObject.GetComponent<IInteractorEntryPoint>());
            focusManagerScript.PopFocus();
            Hide();
        }
    }
}