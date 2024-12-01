using System;
using _Root.Scripts.Model.Focus.Runtime;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime.Focus
{
    [DisallowMultipleComponent]
    [SelectionBase]
    public class FocusAndInteractorEntryPointComponent : MonoBehaviour, IFocusEntryPoint
    {
        [SerializeField] private bool isMain;
        [SerializeField] private bool isFocused;

        [FormerlySerializedAs("focusProcessor")] [SerializeField]
        private FocusProcessorScript focusProcessorScript;

        [SerializeField] private FocusManagerScript focusManagerScript;

        public event Action<GameObject> OnPushFocus;

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
            OnPushFocus?.Invoke(gameObject);
            focusProcessorScript.SetFocus(focusReferences, this.GetCancellationTokenOnDestroy());
        }

        public void RemoveFocus(GameObject targetGameObject)
        {
            isFocused = false;
            focusProcessorScript.OnFocusLost(targetGameObject);
            OnPushFocus?.Invoke(gameObject);
        }

        private void FocusSelf()
        {
            focusManagerScript.PushFocus(this);
        }

        [Button]
        public void SetFocus()
        {
            IsFocused = true;
        }
    }
}