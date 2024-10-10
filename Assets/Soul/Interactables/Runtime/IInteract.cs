using System;

namespace Soul.Interactables.Runtime
{
    public interface IInteract<in T>
    {
        public bool CanInteract(T initiator);
        public void OnInteractHover(T initiator);
        public void OnInteractStart(T initiator, Action onComplete);
        public void OnInteractExit(T initiator);
    }
}