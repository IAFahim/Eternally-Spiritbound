using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractable
    {
        Transform Transform { get; }
        public void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint);
    }
}