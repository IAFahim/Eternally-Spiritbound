using System;
using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractableEntryPoint
    {
        public event Action<IInteractorEntryPoint> OnInteractionStartedEvent;
        public event Action<IInteractorEntryPoint> OnInteractionEndedEvent;
        public void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint);
        Transform Transform { get; }
        public void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint);
    }
}