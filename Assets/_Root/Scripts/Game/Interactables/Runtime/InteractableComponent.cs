using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractableComponent : MonoBehaviour, IInteract
    {
        public void OnInteractEnter()
        {
            Debug.Log("InteractEnter");
        }

        public void OnInteractExit()
        {
            Debug.Log("InteractExit");
        }
    }
}