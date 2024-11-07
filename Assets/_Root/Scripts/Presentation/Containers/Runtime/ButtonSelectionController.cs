using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class ButtonSelectionController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Image lockedImage;

        private int _index;
        private UnityAction<int> _unityAction;

        public void Set(int index, Sprite sprite, UnityAction<int> onClick, bool unlocked)
        {
            Clear();
            _index = index;
            icon.sprite = sprite;
            _unityAction = onClick;
            button.onClick.AddListener(OnClick);
            lockedImage.gameObject.SetActive(unlocked);
        }

        private void OnClick()
        {
            _unityAction?.Invoke(_index);
        }

        public void Clear()
        {
            if (_unityAction != null) button.onClick.RemoveListener(OnClick);
        }

        private void Reset()
        {
            button = GetComponent<Button>();
            var images = GetComponentsInChildren<Image>(true);
            
            icon = images[1];
            lockedImage = images[2];
        }
    }
}