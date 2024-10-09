using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public abstract class InteractableScriptable : ScriptableObject, IInteractableByGameObject
    {
        public abstract void OnInteractStart(GameObject initiator);
        public abstract void OnInteractEnd(GameObject initiator);
    }
}