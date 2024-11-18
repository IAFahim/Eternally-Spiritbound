using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractorEntryPoint : IFocus
    {
        public bool IsMain { get; }
        public GameObject GameObject { get; }
    }
}