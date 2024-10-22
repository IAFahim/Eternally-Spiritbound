using _Root.Scripts.Game.FocusProvider.Runtime;
using Soul.Interactables.Runtime;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IInteractable : IInteractableBase<IInteractor>
    {
        public void OnInteractStart(FocusScriptable focusScriptable);
        public void OnInteractEnd(FocusScriptable focusScriptable);
    }
}