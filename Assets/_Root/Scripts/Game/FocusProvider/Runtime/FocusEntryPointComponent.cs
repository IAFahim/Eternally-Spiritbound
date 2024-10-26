using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusEntryPointComponent : MonoBehaviour, IFocusEntryPoint
    {
        [SerializeField] private bool isMain;
        [SerializeField] private bool isFocused;

        [FormerlySerializedAs("focusControllerScriptable")] [SerializeField]
        private FocusControllerScriptable mainFocusController;

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
            mainFocusController.SetFocus(focusReferences);
            OnPushFocus?.Invoke(gameObject);
        }

        public void RemoveFocus(GameObject targetGameObject)
        {
            isFocused = false;
            mainFocusController.OnFocusLost(targetGameObject);
            OnPushFocus?.Invoke(gameObject);
        }

        private void FocusSelf()
        {
            FocusScriptable.Instance.PushFocus(this);
        }
    }
}