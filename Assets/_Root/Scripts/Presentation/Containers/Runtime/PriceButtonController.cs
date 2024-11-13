using System;
using Pancake.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class PriceButtonController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image priceTypeImage;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text actionText;
        [SerializeField] private float disableAlpha = 0.5f;

        private UnityAction _onClick;

        public void Initialize(Sprite priceTypeIcon, int price, string action, bool hasEnough,
            UnityAction onClick)
        {
            priceTypeImage.sprite = priceTypeIcon;
            AddListener(onClick);
            actionText.text = action;
            SetPrice(price);
            HasEnough(hasEnough);
        }

        public void SetPrice(int price)
        {
            if (price == 0) priceText.text = "Free";
            else priceText.text = price.ToString();
        }

        public void HasEnough(bool status)
        {
            if (status)
            {
                button.image.color = button.colors.normalColor;
                priceText.color = priceText.color.ChangeAlpha(disableAlpha);
                actionText.color = actionText.color.ChangeAlpha(disableAlpha);
                priceTypeImage.color = priceTypeImage.color.ChangeAlpha(disableAlpha);
            }
            else
            {
                button.image.color = button.colors.disabledColor;
                priceText.color = priceText.color.ChangeAlpha(1);
                actionText.color = actionText.color.ChangeAlpha(1);
                priceTypeImage.color = priceTypeImage.color.ChangeAlpha(1);
            }
        }

        private void OnDisable() => RemoveOldListener();

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
            button ??= GetComponent<Button>();
            var images = GetComponentsInChildren<Image>();
            priceTypeImage ??= images[^1];
            var texts = GetComponentsInChildren<TMP_Text>();
            actionText ??= texts[0];
            priceText ??= texts[1];
        }
    }
}