using System;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public abstract class InteractableComponent : MonoBehaviour, IInteractableByGameObject
    {
        public abstract void OnInteractHover(GameObject initiator);
        public abstract bool CanInteract(GameObject initiator);
        public abstract void OnInteractStart(GameObject initiator, Action onComplete);
        public abstract void OnInteractExit(GameObject initiator);
    }
}