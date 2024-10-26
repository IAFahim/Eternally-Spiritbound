using System;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IFocusEntryPoint: IInteractorEntryPoint
    {
        public void PushFocus(FocusReferences focusReferences);
        public void RemoveFocus(GameObject targetGameObject);
        
        public event Action<GameObject> OnPushFocus;
        public event Action<GameObject> OnRemoveFocus;
    }
}