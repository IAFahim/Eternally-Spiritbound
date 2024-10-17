using System;

namespace Soul.Interactables.Runtime
{
    public interface IInteract<in T>
    {
        public bool CanInteract(T initiator);
        public void OnInteractHoverEnter(T initiator);
        public void OnInteractStart(T initiator, Action onComplete);
        public void OnHoverExit(T initiator);
    }
}