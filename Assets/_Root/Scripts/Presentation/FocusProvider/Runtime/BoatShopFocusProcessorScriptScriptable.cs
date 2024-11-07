using System;
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
        public ScriptablePool scriptablePool;
        private Button _closeButton;

        public int equipIndex;
        public string equippedBoatName;
        public string[] unlockedBoatNames;
        public ButtonSelectionController[] _buttonSelectionControllers;

        public override void SetFocus(FocusReferences focusReferences)
        {
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
            var boatInfoDTOs = new BoatInfoDto[boatVehicleAssets.Length];
            for (var i = 0; i < boatVehicleAssets.Length; i++)
            {
                var boatVehicle = boatVehicleAssets[i];
                boatInfoDTOs[i] = new BoatInfoDto
                {
                    index = i,
                    unlocked = Array.Exists(unlockedBoatNames, s => s == boatVehicle.Value),
                    boatVehicleAsset = boatVehicle
                };
            }

            Array.Sort(boatInfoDTOs, Comparison);
            _buttonSelectionControllers = Pool(scrollRectTransform, boatInfoDTOs);
        }

        private int Comparison(BoatInfoDto a, BoatInfoDto b)
        {
            //show unlocked first but keep the order
            if (a.unlocked && !b.unlocked) return -1;
            if (!a.unlocked && b.unlocked) return 1;
            return a.index.CompareTo(b.index);
            
        }

        private ButtonSelectionController[] Pool(Transform scrollRectTransform, BoatInfoDto[] boatInfoDTOs)
        {
            var buttonSelectionControllers = new ButtonSelectionController[boatVehicleAssets.Length];
            for (var i = 0; i < boatInfoDTOs.Length; i++)
            {
                var boatInfoDto = boatInfoDTOs[i];
                var buttonSelectionController = scriptablePool
                    .Request(buttonSelectionControllerAsset, scrollRectTransform)
                    .GetComponent<ButtonSelectionController>();
                buttonSelectionController.Set(
                    i,
                    boatInfoDto.boatVehicleAsset.icon,
                    OnSelectionClick,
                    boatInfoDto.unlocked
                );
                buttonSelectionControllers[i] = buttonSelectionController;
            }

            return buttonSelectionControllers;
        }

        private void OnSelectionClick(int index)
        {
            equipIndex = index;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
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

        public struct BoatInfoDto
        {
            public int index;
            public bool unlocked;
            public BoatVehicleAsset boatVehicleAsset;

            public override string ToString()
            {
                return
                    $"{nameof(index)}: {index}, {nameof(unlocked)}: {unlocked}";
            }
        }
    }
}