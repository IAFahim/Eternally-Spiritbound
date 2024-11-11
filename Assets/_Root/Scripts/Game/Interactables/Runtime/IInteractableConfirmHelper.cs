using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IInteractableConfirmHelper
    {
        public GameObject GameObject { get; }
        public void Active(IInteractableEntryPoint interactableEntryPoint);
        public void Hide();
    }
}