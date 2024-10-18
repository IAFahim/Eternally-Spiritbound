using System;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public abstract class InteractableComponent : MonoBehaviour, IInteractable
    {
        public abstract bool CanInteract(IInteractor initiator);
        public abstract void OnInteractHoverEnter(IInteractor initiator);
        public abstract void OnInteractStart(IInteractor initiator);
        public abstract void OnInteractEnd(IInteractor initiator);

        public abstract void OnHoverExit(IInteractor initiator);
    }
}