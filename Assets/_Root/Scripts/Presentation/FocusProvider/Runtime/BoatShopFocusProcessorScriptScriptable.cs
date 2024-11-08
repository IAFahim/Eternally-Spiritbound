using System;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Boats.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Pancake.Common;
using Soul.Pools.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [SerializeField] private AssetReferenceGameObject boatShopCloseButton;
        [SerializeField] private AssetReferenceGameObject boatScrollRect;
        [SerializeField] private FocusManagerScript focusManager;
        [SerializeField] private AssetReferenceGameObject buttonSelectionControllerAsset;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite equippedSprite;

        [SerializeField] private BoatVehicleAsset[] boatVehicleAssets;
        [SerializeField] private ScriptablePool scriptablePool;
        private Button _closeButton;

        [SerializeField] private string equippedBoatName;
        [SerializeField] private string[] unlockedBoatNames;
        [SerializeField] private BoatInfoDto[] _boatInfoDTOs;
        [SerializeField] private ButtonSelectionController[] _buttonSelectionControllers;

        private ScrollRect _scrollRect;

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
            _scrollRect = gameObject.GetComponent<ScrollRect>();
            _boatInfoDTOs = CreateBoatInfoDto();
            Array.Sort(_boatInfoDTOs, Comparison);
            PopulatePool(_scrollRect, _boatInfoDTOs);
        }

        private BoatInfoDto[] CreateBoatInfoDto()
        {
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

            return boatInfoDTOs;
        }

        private int Comparison(BoatInfoDto a, BoatInfoDto b)
        {
            //show unlocked first but keep the order
            if (a.unlocked && !b.unlocked) return -1;
            if (!a.unlocked && b.unlocked) return 1;
            return a.index.CompareTo(b.index);
        }

        private void PopulatePool(ScrollRect scrollRect, BoatInfoDto[] boatInfoDTOs)
        {
            _buttonSelectionControllers = new ButtonSelectionController[boatVehicleAssets.Length];
            var scrollContentTransform = scrollRect.content.transform;
            for (var i = 0; i < boatInfoDTOs.Length; i++)
            {
                var boatInfoDto = boatInfoDTOs[i];
                var spawnedGameObject = scriptablePool
                    .Request(buttonSelectionControllerAsset, scrollContentTransform);
                var buttonSelectionController = spawnedGameObject.GetComponent<ButtonSelectionController>();
                
                buttonSelectionController.Set(
                    i,
                    boatInfoDto.boatVehicleAsset.icon,
                    StatusSprite(i),
                    SetEquipped
                );
                _buttonSelectionControllers[i] = buttonSelectionController;
            }
        }

        private void SetEquipped(int i)
        {
            StatusSprite(i);
        }

        private Sprite StatusSprite(int index)
        {
            if (equippedBoatName == _boatInfoDTOs[index].boatVehicleAsset.Value)
            {
                SetEquipped(index);
                _scrollRect.ScrollTo(_buttonSelectionControllers[index].transform);
                return equippedSprite;
            }
            
            return _boatInfoDTOs[index].unlocked ? null : lockedSprite;
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

        public class BoatInfoDto
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