using System;
using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractableEntryPoint
    {
        Transform Transform { get; }
        public event Action<IInteractorEntryPoint> OnEnteredEvent;
        public event Action<IInteractorEntryPoint> OnExitEvent;
        public event Action<IInteractorEntryPoint> OnStartedEvent;
        public event Action<IInteractorEntryPoint> OnEndedEvent;
        public void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint);
        public void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint);
    }
}