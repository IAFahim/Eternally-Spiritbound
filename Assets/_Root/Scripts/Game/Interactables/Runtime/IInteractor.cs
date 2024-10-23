using _Root.Scripts.Game.FocusProvider.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IInteractor: IFocusAble
    {
        public GameObject GameObject { get; }
        public void OnInteractStart(IInteractable interactable);
        public void OnInteractEnd(IInteractable interactable);
    }
}