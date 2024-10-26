using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptable : FocusProcessorCinemachineScriptable
    {
        public AssetReferenceGameObject boatShopCloseButton;
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

        private void SetupCloseButton(GameObject gameObject)
        {
            FocusManager.Instance.PeekFocus().OnPushFocus += PushFocus;
            _closeButton = gameObject.GetComponent<Button>();
            _closeButton.onClick.AddListener(TryPopAndActiveLast);
        }

        private void PushFocus(GameObject obj) => OnFocusLost(obj);


        public override void OnFocusLost(GameObject targetGameObject)
        {
            base.OnFocusLost(targetGameObject);
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
        }

        private void TryPopAndActiveLast()
        {
            FocusManager.Instance.PopFocus();
        }
    }
}