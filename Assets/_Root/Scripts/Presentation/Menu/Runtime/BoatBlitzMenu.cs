using System;
using Pancake;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Menu.Runtime
{
    public class BoatBlitzMenu : MonoBehaviour
    {
        public Button settingsButton;
        [Space, SerializeField, PopupPickup] private string settingPopupKey;

        private void OnEnable()
        {
            settingsButton.onClick.AddListener(OnButtonSettingPressed);
        }

        private void OnDisable()
        {
            settingsButton.onClick.RemoveListener(OnButtonSettingPressed);
        }

        private void OnButtonSettingPressed()
        {
            MainUIContainer.In.GetMain<PopupContainer>().Push(settingPopupKey, true);
        }
    }
}