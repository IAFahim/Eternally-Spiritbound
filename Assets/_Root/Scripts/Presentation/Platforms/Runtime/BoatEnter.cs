using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Presentation.Platforms.Runtime
{
    public class BoatEnter : MonoBehaviour, IInteractable
    {
        public GameObject boat;
        [FormerlySerializedAs("mainObjectProviderScriptable")] public MainProviderScriptable mainProviderScriptable;
        public bool CanInteract(IInteractor initiator) => true;

        public void OnInteractHoverEnter(IInteractor initiator)
        {
            if (initiator.GameObject == mainProviderScriptable.mainGameObjectInstance)
                mainProviderScriptable.ProvideTo(boat);
        }

        public void OnInteractStart(IInteractor initiator)
        {
        }

        public void OnInteractEnd(IInteractor initiator)
        {
        }

        public void OnHoverExit(IInteractor initiator)
        {
        }
    }
}