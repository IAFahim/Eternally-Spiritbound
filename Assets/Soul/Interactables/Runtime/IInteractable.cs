using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractable
    {
        Transform Transform { get; }
        public void OnInteractableDetected(IInteractor interactor);
        public void OnInteractableDetectionLost(IInteractor interactor);
        public void OnInteractionStarted(IInteractor interactor);
        public void OnInteractionEnded(IInteractor interactor);
    }
}