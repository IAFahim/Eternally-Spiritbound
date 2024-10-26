using System;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusEntryPointComponent : MonoBehaviour, IFocusEntryPoint
    {
        [SerializeField] private bool isMain;
        [SerializeField] private bool isFocused;
        
        [SerializeField]
        private FocusProcessor focusProcessor;

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
            focusProcessor.SetFocus(focusReferences);
            OnPushFocus?.Invoke(gameObject);
        }

        public void RemoveFocus(GameObject targetGameObject)
        {
            isFocused = false;
            focusProcessor.OnFocusLost(targetGameObject);
            OnPushFocus?.Invoke(gameObject);
        }

        private void FocusSelf()
        {
            FocusManager.Instance.PushFocus(this);
        }
    }
}