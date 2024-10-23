using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusEntryPointComponent : MonoBehaviour, IFocusConsumer
    {
        [SerializeField] private FocusConsumerScriptable focusConsumerScriptable;

        public bool IsFocused
        {
            get => focusConsumerScriptable.IsFocused;
            private set => focusConsumerScriptable.IsFocused = value;
        }

        private Action _returnCallback;

        public void SetFocus(FocusReferences focusReferences)
        {
            IsFocused = true;
            focusConsumerScriptable.SetFocus(focusReferences);
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            IsFocused = false;
            focusConsumerScriptable.OnFocusLost(targetGameObject);
        }
    }
}