using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableComponent : MonoBehaviour, IInteractableByGameObject
    {
        public InteractableScriptable interactableScriptable;

        public void OnInteractStart(GameObject initiator)
        {
            interactableScriptable.OnInteractStart(initiator);
        }

        public void OnInteractEnd(GameObject initiator)
        {
            interactableScriptable.OnInteractEnd(initiator);
        }
    }
}