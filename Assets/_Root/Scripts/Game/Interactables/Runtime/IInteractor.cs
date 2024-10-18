using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IInteractor
    {
        public GameObject GameObject { get; }
        public void Interact();
    }
}