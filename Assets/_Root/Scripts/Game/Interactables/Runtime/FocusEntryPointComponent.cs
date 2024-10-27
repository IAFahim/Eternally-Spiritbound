﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusEntryPointComponent : MonoBehaviour, IFocusEntryPoint
    {
        [SerializeField] private bool isMain;
        [SerializeField] private bool isFocused;
        
        [FormerlySerializedAs("focusProcessor")] [SerializeField]
        private FocusProcessorScript focusProcessorScript;

        public event Action<GameObject> OnPushFocus;
        public event Action<GameObject> OnRemoveFocus;

        public bool IsMain => isMain;
        public GameObject GameObject => gameObject;

        public bool IsFocused
        {
            get => isFocused;
            set
            {
                if (isFocused == value) return;
                isFocused = value;
                if (value) FocusSelf();
                else RemoveFocus(GameObject);
            }
        }

        public void PushFocus(FocusReferences focusReferences)
        {
            isFocused = true;
            focusProcessorScript.SetFocus(focusReferences);
            OnPushFocus?.Invoke(gameObject);
        }

        public void RemoveFocus(GameObject targetGameObject)
        {
            isFocused = false;
            focusProcessorScript.OnFocusLost(targetGameObject);
            OnPushFocus?.Invoke(gameObject);
        }

        private void FocusSelf()
        {
            FocusManagerScript.Instance.PushFocus(this);
        }
    }
}