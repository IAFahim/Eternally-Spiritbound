using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class ButtonSelectionController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;

        [FormerlySerializedAs("lockedImage")] [SerializeField]
        private Image statusImage;

        private int _index;
        private UnityAction<int> _unityAction;

        public void Set(int index, Sprite sprite, Sprite statusSprite, UnityAction<int> onClick)
        {
            Clear();
            _index = index;
            icon.sprite = sprite;
            SetStatus(statusSprite);
            _unityAction = onClick;
            button.onClick.AddListener(OnClick);
        }

        private void SetStatus(Sprite statusSprite)
        {
            var statusNull = statusSprite == null;
            if (!statusNull) statusImage.sprite = statusSprite;
            statusImage.enabled = !statusNull;
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
            statusImage = images[2];
        }
    }
}