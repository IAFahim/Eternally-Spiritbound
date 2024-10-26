using UnityEngine;

namespace Soul.Interactables.Runtime
{
    public interface IInteractorEntryPoint
    {
        public bool IsMain { get; }
        public bool IsFocused { get; set; }
        public GameObject GameObject { get; }
    }
}