using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractComponent : MonoBehaviour, IInteractable
    {
        public void OnInteractableDetected(IInteractor interactor)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteractableDetectionLost(IInteractor interactor)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteractionStarted(IInteractor interactor)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteractionEnded(IInteractor interactor)
        {
            throw new System.NotImplementedException();
        }
    }
}