using System;
using _Root.Scripts.Model.Assets.Runtime;
using Sisus.Init;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class TabButtonController : MonoBehaviour, IInitializable<int, string, Sprite, UnityAction<int>>
    {
        [SerializeField] private int indexNumber;
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;

        private UnityAction<int> _onIndexClick;

        public void Init(int index, string title, Sprite icon, UnityAction<int> onIndexClicked)
        {
            indexNumber = index;
            titleText.text = title;
            iconImage.sprite = icon;
            _onIndexClick = onIndexClicked;
            AddListener(OnIndexClicked);
        }

        private void OnIndexClicked()
        {
            _onIndexClick?.Invoke(indexNumber);
        }

        public void SetSelected(bool selected)
        {
            if (selected) button.Select();
            else button.OnDeselect(null);
        }

        private void OnDisable() => button.onClick.RemoveAllListeners();

        private void AddListener(UnityAction onClick)
        {
            RemoveOldListener();
            button.onClick.AddListener(onClick);
        }

        private void RemoveOldListener()
        {
            if (_onIndexClick != null)
            {
                button.onClick.RemoveListener(OnIndexClicked);
                _onIndexClick = null;
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