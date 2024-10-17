using System;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IInteractorGameObject
    {
        public event Action<IInteractableWithGameObject> OnInteractorFound;
    }
}