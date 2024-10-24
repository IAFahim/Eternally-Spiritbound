using System;
using UnityEngine;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusEntryPointComponent : MonoBehaviour, IFocusConsumer, IFocus
    {
        [SerializeField] private bool isFocused;
        [SerializeField] private FocusControllerScriptable focusControllerScriptable;

        public bool IsFocused
        {
            get => isFocused;
            private set => isFocused = value;
        }

        public void SetFocus(FocusReferences focusReferences)
        {
            IsFocused = true;
            focusControllerScriptable.SetFocus(focusReferences);
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            IsFocused = false;
            focusControllerScriptable.OnFocusLost(targetGameObject);
        }
    }
}