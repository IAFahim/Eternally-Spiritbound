using Sisus.Init;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime.Helper
{
    public interface IInteractableConfirmHelper : IInitializable<IInteractableEntryPoint, Vector3[]>
    {
        public GameObject GameObject { get; }
        public void Hide();
    }
}