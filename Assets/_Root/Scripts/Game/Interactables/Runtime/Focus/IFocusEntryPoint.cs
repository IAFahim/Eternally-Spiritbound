using System;
using _Root.Scripts.Model.Focus.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime.Focus
{
    public interface IFocusEntryPoint: IInteractorEntryPoint
    {
        public void PushFocus(FocusReferences focusReferences);
        public void RemoveFocus(GameObject targetGameObject);
        
        public event Action<GameObject> OnPushFocus;
    }
}