using System;
using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.MainProviders.Runtime
{
    public class FocusConsumerComponent : MonoBehaviour, IFocusConsumer
    {
        [SerializeField] private FocusConsumerScriptable focusConsumerScriptable;

        public bool IsFocused
        {
            get => focusConsumerScriptable.IsFocused;
            private set => focusConsumerScriptable.IsFocused = value;
        }

        private Action _returnCallback;

        public void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences,
            GameObject targetGameObject)
        {
            IsFocused = true;
            focusConsumerScriptable.SetFocus(activeElements, transformReferences, targetGameObject);
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            IsFocused = false;
            focusConsumerScriptable.OnFocusLost(targetGameObject);
        }
    }
}