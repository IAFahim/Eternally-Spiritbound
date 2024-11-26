using Cysharp.Threading.Tasks;
using Pancake;
using Pancake.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.Popups.Setting
{
    public class SettingView : View
    {
        [SerializeField] private Button buttonClose;

        [SerializeField, PopupPickup] private string creditPopupKey;
        [SerializeField, PopupPickup] private string backupDataPopupKey;

        protected override UniTask Initialize()
        {
            buttonClose.onClick.AddListener(OnButtoClosePressed);
            return UniTask.CompletedTask;
        }

        private void OnButtonBackupPressed()
        {
            MainUIContainer.In.GetMain<PopupContainer>().PushAsync(backupDataPopupKey, true).Forget();
        }

        private void OnButtonCreditPressed()
        {
            MainUIContainer.In.GetMain<PopupContainer>().PushAsync(creditPopupKey, true).Forget();
        }


        private async void OnButtoClosePressed()
        {
            PlaySoundClose();
            await PopupHelper.Close(transform);
        }
    }
}