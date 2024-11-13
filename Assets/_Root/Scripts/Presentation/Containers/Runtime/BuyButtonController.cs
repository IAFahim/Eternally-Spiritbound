using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Containers.Runtime
{
    public class BuyButtonController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text priceText;

        private Action _onClick;

        public void Initialize(Sprite icon, int price, bool hasEnough, UnityAction onClick)
        {
            image.sprite = icon;
            button.onClick.AddListener(onClick);
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
            if (!status)
            {
                button.image.color = button.colors.disabledColor;
            }
            else
            {
                button.image.color = button.colors.normalColor;
            }
        }
    }
}