using System;
using _Root.Scripts.Game.Infrastructures.Runtime.Shops;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Utils.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Boats.Runtime;
using _Root.Scripts.Model.Relationships.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Pancake.Common;
using Pancake.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Pancake.Pools;
using Soul.Pools.Runtime;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [SerializeField] private AssetReferenceGameObject boatShopCloseButtonAsset;
        [SerializeField] private AssetReferenceGameObject boatScrollRectAsset;

        [SerializeField] private FocusManagerScript focusManager;
        [SerializeField] private AssetReferenceGameObject buttonSelectionControllerAsset;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite equippedSprite;

        private Button _closeButton;
        [SerializeField] private AssetOwnsAssetsLink assetOwnsAssetsLink;

        private AssetScript[] _unlockedBoatAssets;
        private ButtonSelectionController[] _buttonSelectionControllers;
        private BoatVehicleAsset[] _boatVehicleAssets;
        private AssetScriptComponent _assetScriptComponent;
        private ScrollRect _scrollRect;
        private BoatShop _boatShopBase;
        private BoatInfoDto[] _boatInfoDTOs;
        private TMP_Text _titleText;
        private int _lastSelected;

        private string Key => focusManager.mainObject.name + "BoatShop";

        public override void SetFocus(FocusReferences focusReferences)
        {
            TargetGameObject = focusReferences.currentGameObject;
            BuildCache(
                focusReferences.ActiveElements,
                (cinemachineAsset, SetupCinemachine, null),
                (boatShopCloseButtonAsset, SetupCloseButton, focusReferences.stillCanvasTransformPoint),
                (boatScrollRectAsset, SetupScrollRect, focusReferences.stillCanvasTransformPoint)
            );
        }

        private void SetupScrollRect(GameObject gameObject)
        {
            _lastSelected = 0;
            _scrollRect = gameObject.GetComponent<ScrollRect>();
            _titleText = gameObject.GetComponentInChildren<TMP_Text>();
            _assetScriptComponent = focusManager.mainObject.GetComponent<AssetScriptComponent>();
            OnBoatMenu();
        }

        private void OnBoatMenu()
        {
            _boatShopBase = TargetGameObject.GetComponent<BoatShop>();
            _unlockedBoatAssets = assetOwnsAssetsLink[_assetScriptComponent.assetScript].ToArray();
            _boatVehicleAssets = _boatShopBase.GetItems().ToArray();
            _boatInfoDTOs = CreateBoatInfoDto(_boatVehicleAssets);
            Array.Sort(_boatInfoDTOs);
            PopulatePool(_scrollRect, _boatInfoDTOs);
        }

        private BoatInfoDto[] CreateBoatInfoDto(BoatVehicleAsset[] boatVehicles)
        {
            var boatInfoDTOs = new BoatInfoDto[boatVehicles.Length];
            for (var i = 0; i < boatVehicles.Length; i++)
            {
                var boatVehicle = boatVehicles[i];
                boatInfoDTOs[i] = new BoatInfoDto
                {
                    Index = i,
                    Unlocked = _unlockedBoatAssets.Any(asset => asset.guid == boatVehicle.guid),
                    BoatVehicleAsset = boatVehicle
                };
            }

            return boatInfoDTOs;
        }


        private void PopulatePool(ScrollRect scrollRect, BoatInfoDto[] boatInfoDTOs)
        {
            _buttonSelectionControllers = new ButtonSelectionController[_boatVehicleAssets.Length];
            var scrollContentTransform = scrollRect.content.transform;
            for (var i = 0; i < boatInfoDTOs.Length; i++)
            {
                _buttonSelectionControllers[i] = CreateController(i, scrollContentTransform, boatInfoDTOs[i],
                    out var isEquipped);
                Debug.Log(boatInfoDTOs[i] + " " + _buttonSelectionControllers[i].transform.GetSiblingIndex());
                if (isEquipped) _lastSelected = i;
            }

            foreach (var buttonSelectionController in _buttonSelectionControllers)
            {
                buttonSelectionController.gameObject.SetActive(true);
            }

            Select(_lastSelected);
        }


        private ButtonSelectionController CreateController(int index,
            Transform scrollContentTransform,
            BoatInfoDto boatInfoDto,
            out bool isEquipped)
        {
            var buttonSelectionController = SharedAssetReferencePoolInactive
                .Request(buttonSelectionControllerAsset, scrollContentTransform)
                .GetComponent<ButtonSelectionController>();
            buttonSelectionController.transform.SetSiblingIndex(index);
            var equippedBoatGuid = PlayerPrefs.GetString(Key, string.Empty);
            isEquipped = equippedBoatGuid == boatInfoDto.BoatVehicleAsset.guid;
            buttonSelectionController.Initialize(
                index, isEquipped,
                boatInfoDto.BoatVehicleAsset.icon,
                StatusSprite(isEquipped, boatInfoDto.Unlocked),
                Select
            );
            return buttonSelectionController;
        }


        private void Select(int index)
        {
            if (_lastSelected != index)
            {
                if (_boatInfoDTOs[index].Unlocked) DeSelect(_lastSelected);
                else NotifyDeSelect(_lastSelected);
            }

            SetSelected(index);
        }

        private void SetSelected(int index)
        {
            _lastSelected = index;
            bool unlocked = _boatInfoDTOs[index].Unlocked;
            _titleText.text = _boatInfoDTOs[index].BoatVehicleAsset.Value;
            var buttonSelectionController = _buttonSelectionControllers[index];
            if (unlocked) NotifyUnLockedSelected(index);
            else NotifyLockedSelect(index);
            var normalizedPosition = _scrollRect.ScrollNormalizedPosition(buttonSelectionController.transform);
            Debug.Log(normalizedPosition);
        }

        private void DeSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(StatusSprite(false, _boatInfoDTOs[index].Unlocked));
            NotifyDeSelect(index);
        }

        private void NotifyUnLockedSelected(int index)
        {
            PlayerPrefs.SetString(Key, _boatInfoDTOs[index].BoatVehicleAsset.guid);
            _buttonSelectionControllers[index].SetStatus(equippedSprite);
            _boatShopBase.OnUnlockedSelected(_boatInfoDTOs[index].BoatVehicleAsset);
        }

        private void NotifyLockedSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(lockedSprite);
            _boatShopBase.OnLockedItemSelected(_boatInfoDTOs[index].BoatVehicleAsset);
        }

        private void NotifyDeSelect(int index)
        {
            _buttonSelectionControllers[index].DeSelect();
            _boatShopBase.OnDeSelected(_boatInfoDTOs[index].BoatVehicleAsset);
        }

        private Sprite StatusSprite(bool isEquipped, bool unlocked)
        {
            if (!unlocked) return lockedSprite;
            if (isEquipped) return equippedSprite;
            return null;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            foreach (var buttonSelectionController in _buttonSelectionControllers)
            {
                SharedAssetReferencePoolInactive.Return(buttonSelectionControllerAsset,
                    buttonSelectionController.gameObject);
            }
        }

        private void SetupCloseButton(GameObject gameObject)
        {
            focusManager.PeekFocus().OnPushFocus += OnFocusLost;
            _closeButton = gameObject.GetComponent<Button>();
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            _closeButton.onClick.AddListener(TryPopAndActiveLast);
        }

        private void TryPopAndActiveLast() => focusManager.PopFocus();

        private struct BoatInfoDto : IComparable<BoatInfoDto>
        {
            public int Index;
            public bool Unlocked;
            public BoatVehicleAsset BoatVehicleAsset;

            public override string ToString()
            {
                return
                    $"{nameof(Index)}: {Index}, {nameof(Unlocked)}: {Unlocked}";
            }

            public int CompareTo(BoatInfoDto other)
            {
                if (Unlocked && !other.Unlocked) return -1;
                if (!Unlocked && other.Unlocked) return 1;
                return Index.CompareTo(other.Index);
            }
        }
    }
}