using System;

namespace Soul.Interactables.Runtime
{
    public interface IInteractableBase<in T>
    {
        public bool CanInteract(T initiator);
        public void OnInteractHoverEnter(T initiator);
        public void OnInteractStart(T initiator);
        public void OnInteractEnd(T initiator);
        public void OnHoverExit(T initiator);
    }
}