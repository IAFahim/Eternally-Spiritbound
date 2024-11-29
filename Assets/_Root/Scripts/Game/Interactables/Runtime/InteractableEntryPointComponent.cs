using System;
using _Root.Scripts.Game.Interactables.Runtime.Helper;
using Pancake;
using Pancake.Pools;
using Soul.Interactables.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableEntryPointComponent : MonoBehaviour, IInteractableEntryPoint
    {
        private IInteractorEntryPoint _interactorEntryPoint;
        public Transform Transform => transform;

        public event Action<IInteractorEntryPoint> OnEnteredEvent;
        public event Action<IInteractorEntryPoint> OnExitEvent;
        public event Action<IInteractorEntryPoint> OnStartedEvent;
        public event Action<IInteractorEntryPoint> OnEndedEvent;


        private void Awake() => _interactorEntryPoint = GetComponent<IInteractorEntryPoint>();

        public virtual void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint)
        {
            OnStartedEvent?.Invoke(interactorEntryPoint);
            if (interactorEntryPoint.IsFocused) _interactorEntryPoint.IsFocused = true;
        }

        public virtual void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint)
        {
            OnEndedEvent?.Invoke(interactorEntryPoint);
            if (interactorEntryPoint.IsFocused) _interactorEntryPoint.IsFocused = false;
        }

        public virtual void OnInteractableDetected(IInteractorEntryPoint interactorEntryPoint)
        {
            OnEnteredEvent?.Invoke(interactorEntryPoint);
        }


        public virtual void OnInteractableDetectionLost(IInteractorEntryPoint interactorEntryPoint)
        {
            OnExitEvent?.Invoke(interactorEntryPoint);
        }
    }
}