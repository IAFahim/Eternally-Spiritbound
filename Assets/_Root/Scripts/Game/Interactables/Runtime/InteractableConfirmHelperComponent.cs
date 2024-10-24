using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Selectors.Runtime;
using Soul.Interactables.Runtime;
using Soul.Selectors.Runtime;
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
            _interactableParent.OnInteractionStarted(FocusScriptable.Instance.mainObject.GetComponent<IInteractor>());
            selectedEvent.Invoke();
        }

        private void InteractEnd()
        {
            _interactableParent.OnInteractionEnded(FocusScriptable.Instance.mainObject.GetComponent<IInteractor>());
            FocusScriptable.Instance.TryPopAndActiveLast();
            Hide();
        }
    }
}