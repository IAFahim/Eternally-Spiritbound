using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractor
    {
        public bool Focused { get; }
        public GameObject GameObject { get; }
    }
}