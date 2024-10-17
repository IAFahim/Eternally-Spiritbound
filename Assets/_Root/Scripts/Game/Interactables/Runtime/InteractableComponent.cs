using System;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public abstract class InteractableComponent : MonoBehaviour, IInteractableWithGameObject
    {
        public abstract bool CanInteract(GameObject initiator);
        public abstract void OnInteractHoverEnter(GameObject initiator);
        public abstract void OnInteractStart(GameObject initiator, Action onComplete);
        public abstract void OnHoverExit(GameObject initiator);
    }
}