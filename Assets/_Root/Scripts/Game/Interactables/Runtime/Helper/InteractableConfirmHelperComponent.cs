using _Root.Scripts.Game.Interactables.Runtime.Focus;
using Soul.Interactables.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace _Root.Scripts.Game.Interactables.Runtime.Helper
{
    public class InteractableConfirmHelperComponent : MonoBehaviour, ISelectCallBackReceiver, IInteractableConfirmHelper
    {
        [SerializeField] protected FocusManagerScript focusManagerScript;
        protected IInteractableEntryPoint InteractableEntryPointParent;
        
        [SerializeField] private bool endInteractOnDeselect;
        [Space(20)]
        [SerializeField] private UnityEvent activeEvent;
        [SerializeField] private UnityEvent selectedEvent;
        [SerializeField] private UnityEvent deselectedEvent;
        
        private Vector3[] _offset;
        public GameObject GameObject => gameObject;
        
        public virtual void Init(IInteractableEntryPoint interactableEntryPoint, Vector3[] offset)
        {
            InteractableEntryPointParent = interactableEntryPoint;
            _offset = offset;
            activeEvent.Invoke();
        }
        
        private void OnUpdate()
        {
            // check on which side the player is
            var playerPosition = focusManagerScript.mainObject.transform.position;
            var interactablePosition = InteractableEntryPointParent.Transform.position;
            var direction = playerPosition - interactablePosition;
            var closestDistance = float.MaxValue;
            var closestIndex = 0;
            for (var i = 0; i < _offset.Length; i++)
            {
                var directionOffset = _offset[i];
                var distance = Vector3.Distance(direction, directionOffset);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }
            
            transform.position = interactablePosition + _offset[closestIndex];
        }

        public void OnSelected(RaycastHit hit)
        {
            InteractStart();
        }

        public void OnUpdateDrag(RaycastHit hitRef, bool isInside, Vector3 worldPosition, Vector3 worldPositionDelta)
        {
        }

        public void OnDragEnd(RaycastHit hitRef, bool isInside, Vector3 worldPosition)
        {
        }


        public void OnDeselected(RaycastHit lastHitInfo, RaycastHit hit)
        {
            if (endInteractOnDeselect) focusManagerScript.PopFocus();
        }

        public void OnReselected(RaycastHit hit)
        {
            InteractStart();
        }

        public void Active(IInteractableEntryPoint interactableEntryPoint)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Hide()
        {
            deselectedEvent.Invoke();
        }

        private void InteractStart()
        {
            selectedEvent.Invoke();
            InteractableEntryPointParent.OnInteractionStarted(
                focusManagerScript.mainObject.GetComponent<IInteractorEntryPoint>()
            );
        }

        private void InteractEnd()
        {
            InteractableEntryPointParent.OnInteractionEnded(focusManagerScript.mainObject
                .GetComponent<IInteractorEntryPoint>());
            Hide();
        }
        
        private void OnDisable()
        {
            Hide();
        }
    }
}