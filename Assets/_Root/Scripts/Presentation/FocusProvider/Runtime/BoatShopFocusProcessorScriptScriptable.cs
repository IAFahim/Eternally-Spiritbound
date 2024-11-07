using System.Collections.Generic;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Boats.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        public AssetReferenceGameObject boatShopCloseButton;
        public AssetReferenceGameObject boatScrollRect;
        public FocusManagerScript focusManager;
        public AssetReferenceGameObject buttonSelectionControllerAsset;

        public BoatVehicleAsset[] boatVehicleAssets;
        private List<ButtonSelectionController> _buttonSelectionControllers;
        public ScriptablePool scriptablePool;

        private Button _closeButton;

        public override void SetFocus(FocusReferences focusReferences)
        {
            _buttonSelectionControllers = new();
            TargetGameObject = focusReferences.currentGameObject;
            BuildCache(
                focusReferences.ActiveElements,
                (cinemachineAsset, SetupCinemachine, null),
                (boatShopCloseButton, SetupCloseButton, focusReferences.stillCanvasTransformPoint),
                (boatScrollRect, SetupScrollRect, focusReferences.stillCanvasTransformPoint)
            );
        }

        private void SetupScrollRect(GameObject gameObject)
        {
            var scrollRect = gameObject.GetComponent<ScrollRect>();
            var scrollRectTransform = scrollRect.content.transform;
            foreach (var boatVehicle in boatVehicleAssets)
            {
                var buttonSelectionController = scriptablePool
                    .Request(buttonSelectionControllerAsset, scrollRectTransform)
                    .GetComponent<ButtonSelectionController>();

                buttonSelectionController.Set(0, boatVehicle.icon, OnSelectionClick, true);
                _buttonSelectionControllers.Add(buttonSelectionController);
            }
        }

        private void OnSelectionClick(int arg0)
        {
            Debug.Log($"Selection: {arg0}");
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            focusManager.PeekFocus().OnPushFocus -= OnFocusLost;
            foreach (var buttonSelectionController in _buttonSelectionControllers)
            {
                buttonSelectionController.Clear();
                scriptablePool.Return(buttonSelectionControllerAsset, buttonSelectionController.gameObject);
            }
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