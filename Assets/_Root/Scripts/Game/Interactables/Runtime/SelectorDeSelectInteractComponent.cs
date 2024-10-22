using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Selectors.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class SelectorDeSelectInteractComponent : MonoBehaviour, ISelector
    {
        private IInteractable _interactableParent;
        [SerializeField] private UnityEvent activeEvent;
        [SerializeField] private UnityEvent selectedEvent;
        [SerializeField] private UnityEvent deselectedEvent;

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
            _interactableParent.OnInteractEnd(info);
            deselectedEvent.Invoke();
        }

        public void OnDeselectedWithoutClick()
        {
            deselectedEvent.Invoke();
        }
        
        private void Pass(FocusScriptable info)
        {
            _interactableParent.OnInteractStart(info);
            selectedEvent.Invoke();
        }

        public void OnReselected(FocusScriptable focusScriptable)
        {
            Pass(focusScriptable);
        }
    }
}