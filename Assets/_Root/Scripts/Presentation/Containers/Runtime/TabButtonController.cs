using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class TabButtonController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;

        private UnityAction _onClick;

        public void Initialize(string title, Sprite icon, bool selected, UnityAction onClick)
        {
            titleText.text = title;
            iconImage.sprite = icon;
            AddListener(onClick);
            SetSelected(selected);
        }

        private void SetSelected(bool selected)
        {
            if(selected) button.Select();
            else button.OnDeselect(null);
        }

        private void OnDisable() => button.onClick.RemoveAllListeners();

        private void AddListener(UnityAction onClick)
        {
            RemoveOldListener();
            _onClick = onClick;
            button.onClick.AddListener(onClick);
        }

        private void RemoveOldListener()
        {
            if (_onClick != null)
            {
                button.onClick.RemoveListener(_onClick);
                _onClick = null;
            }
        }

        private void Reset()
        {
            button = GetComponent<Button>();
            var images = GetComponentsInChildren<Image>();
            iconImage = images[1];
            titleText = GetComponentInChildren<TMP_Text>();
        }
    }
}