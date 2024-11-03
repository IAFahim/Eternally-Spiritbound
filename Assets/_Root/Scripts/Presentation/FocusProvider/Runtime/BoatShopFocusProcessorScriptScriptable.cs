using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        public AssetReferenceGameObject boatShopCloseButton;
        public FocusManagerScript focusManager;
        private Button _closeButton;

        public override void SetFocus(FocusReferences focusReferences)
        {
            TargetGameObject = focusReferences.currentGameObject;
            BuildCache(
                focusReferences.ActiveElements,
                (cinemachineAsset, SetupCinemachine, null),
                (boatShopCloseButton, SetupCloseButton, focusReferences.stillCanvasTransformPoint)
            );
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            focusManager.PeekFocus().OnPushFocus -= OnFocusLost;
        }

        private void SetupCloseButton(GameObject gameObject)
        {
            focusManager.PeekFocus().OnPushFocus += OnFocusLost;
            _closeButton = gameObject.GetComponent<Button>();
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            _closeButton.onClick.AddListener(TryPopAndActiveLast);
        }

        private void TryPopAndActiveLast()
        {
            focusManager.PopFocus();
        }
    }
}