using _Root.Scripts.Game.FocusProvider.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public abstract class VehicleBaseComponent : MonoBehaviour, IInteractable
    {
        public FocusEntryPointComponent focusEntryPointComponent;
        public GameObject driver;

        public bool CanInteract(IInteractor initiator) => driver == null;

        public abstract void OnInteractHoverEnter(IInteractor initiator);

        public abstract void OnInteractStart(IInteractor initiator);

        public abstract void OnInteractEnd(IInteractor initiator);

        public abstract void OnHoverExit(IInteractor initiator);
        public abstract void OnInteractStart(FocusScriptable focusScriptable);

        public abstract void OnInteractEnd(FocusScriptable focusScriptable);
    }
}