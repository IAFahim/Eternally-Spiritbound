using System;
using UnityEngine;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public interface IFocusEntryPoint: IFocus
    {
        public GameObject GameObject { get; }
        public void PushFocus(FocusReferences focusReferences);
        public void RemoveFocus(GameObject targetGameObject);
        
        public event Action<GameObject> OnPushFocus;
        public event Action<GameObject> OnRemoveFocus;
    }
}