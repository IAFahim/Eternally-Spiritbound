using System;
using System.Collections.Generic;
using _Root.Scripts.Game.GameEntities.Runtime;
using _Root.Scripts.Game.Infrastructures.Runtime.Shops;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Utils.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Relationships.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Pancake.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Soul.Pools.Runtime;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [SerializeField] private AssetReferenceGameObject boatScrollRectAsset;
        [SerializeField] private AssetReferenceGameObject buyButtonAsset;
        [SerializeField] private AssetReferenceGameObject boatShopCloseButtonAsset;

        [SerializeField] private FocusManagerScript focusManager;
        [SerializeField] private AssetReferenceGameObject buttonSelectionControllerAsset;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite equippedSprite;

        private Button _closeButton;
        [SerializeField] private AssetOwnsAssetsLink assetOwnsAssetsLink;

        private List<AssetScript> _unlockedAssets;
        private ButtonSelectionController[] _buttonSelectionControllers;
        private AssetScriptComponent _playerAssetScriptComponent;
        private ScrollRect _scrollRect;
        private ShopBase _boatShopBase;
        private string _category;

        private AssetInfoDto[] _assetInfoDTOs;
        private TMP_Text _titleText;
        private int _lastSelected;


        private string GetSelectedCategory() => PlayerPrefs.GetString(value, string.Empty);
        private string SetSelectedCategory(string category) => PlayerPrefs.GetString(value, category);

        private void SetAssetEquippedOnCategory(string category, string guid) =>
            PlayerPrefs.GetString(focusManager.mainObject.name + category + value, guid);

        private string GetAssetEquippedCategorySelected(string category) =>
            PlayerPrefs.GetString(PlayerEquippedPerCategoryLookUpKey(category), string.Empty);

        private string PlayerEquippedPerCategoryLookUpKey(string category) =>
            focusManager.mainObject.name + category + value;

        public override void SetFocus(FocusReferences focusReferences)
        {
            TargetGameObject = focusReferences.currentGameObject;
            BuildCache(
                focusReferences.ActiveElements,
                (cinemachineAsset, SetupCinemachine, null),
                (boatShopCloseButtonAsset, SetupCloseButton, focusReferences.stillCanvasTransformPoint),
                (boatScrollRectAsset, SetupScrollRect, focusReferences.stillCanvasTransformPoint),
                (buyButtonAsset, SetupBuyButton, focusReferences.stillCanvasTransformPoint)
            );
        }

        private void SetupBuyButton(GameObject obj)
        {
            var buyButton = obj.GetComponent<Button>();
            buyButton.onClick.AddListener(() =>
            {
                var assetScript = _assetInfoDTOs[_lastSelected].AssetScript;
                var buySuccess = _boatShopBase.OnTryBuyButtonClick(_playerAssetScriptComponent, _category, assetScript,
                    out var message);
                Debug.Log($"{buySuccess}: {message}");
            });
        }

        private void SetupScrollRect(GameObject gameObject)
        {
            _lastSelected = 0;
            _scrollRect = gameObject.GetComponent<ScrollRect>();
            _titleText = gameObject.GetComponentInChildren<TMP_Text>();
            _playerAssetScriptComponent = focusManager.mainObject.GetComponent<AssetScriptComponent>();
            InstantiateShopBase();
        }

        private void InstantiateShopBase()
        {
            _boatShopBase = TargetGameObject.GetComponent<ShopBase>();
            var assetCategories = _boatShopBase.assetCategories;
            var selectedCategory = GetSelectedCategory();
            _category = "";
            foreach (var assetCategory in assetCategories)
            {
                if (assetCategory.name != selectedCategory) continue;
                InstantiateCategory(selectedCategory, assetCategory);
                break;
            }

            if (string.IsNullOrEmpty(_category)) InstantiateCategory(assetCategories[0].name, assetCategories[0]);
        }

        private void InstantiateCategory(string selectedCategory, AssetCategory assetCategory)
        {
            _category = selectedCategory;
            bool linkExist =
                assetOwnsAssetsLink.TryGetValue(_playerAssetScriptComponent.assetScriptReference, out _unlockedAssets);
            _assetInfoDTOs = CreateBoatInfoDto(assetCategory.assets, linkExist);
            Array.Sort(_assetInfoDTOs);
            PopulatePool(_scrollRect, assetCategory, _assetInfoDTOs);
        }

        private AssetInfoDto[] CreateBoatInfoDto(List<AssetScript> boatVehicles, bool linkExist)
        {
            var assetInfoDtos = new AssetInfoDto[boatVehicles.Count];
            for (var i = 0; i < boatVehicles.Count; i++)
            {
                var assetScript = boatVehicles[i];
                var unlocked = linkExist && _unlockedAssets.Any(asset => asset.guid == assetScript.guid);
                assetInfoDtos[i] = new AssetInfoDto
                {
                    Index = i,
                    Unlocked = unlocked,
                    AssetScript = assetScript
                };
            }

            return assetInfoDtos;
        }


        private void PopulatePool(ScrollRect scrollRect, AssetCategory assetCategory, AssetInfoDto[] assetInfoDTOs)
        {
            _buttonSelectionControllers = new ButtonSelectionController[assetCategory.assets.Count];
            var scrollContentTransform = scrollRect.content.transform;
            for (var i = 0; i < assetInfoDTOs.Length; i++)
            {
                _buttonSelectionControllers[i] = CreateController(i, assetCategory.name, scrollContentTransform,
                    assetInfoDTOs[i],
                    out var isEquipped);
                if (isEquipped) _lastSelected = i;
            }

            foreach (var buttonSelectionController in _buttonSelectionControllers)
            {
                buttonSelectionController.gameObject.SetActive(true);
            }

            Select(_lastSelected);
        }


        private ButtonSelectionController CreateController(int index, string category,
            Transform scrollContentTransform,
            AssetInfoDto assetInfoDto,
            out bool isEquipped)
        {
            var buttonSelectionController = SharedAssetReferencePoolInactive
                .Request(buttonSelectionControllerAsset, scrollContentTransform)
                .GetComponent<ButtonSelectionController>();
            buttonSelectionController.transform.SetSiblingIndex(index);
            var equippedAssetGuid = GetAssetEquippedCategorySelected(category);
            isEquipped = equippedAssetGuid == assetInfoDto.AssetScript.guid;
            buttonSelectionController.Initialize(
                index, category, isEquipped,
                assetInfoDto.AssetScript.icon,
                StatusSprite(isEquipped, assetInfoDto.Unlocked),
                Select
            );
            return buttonSelectionController;
        }


        private void Select(int index)
        {
            if (_lastSelected != index)
            {
                if (_assetInfoDTOs[index].Unlocked) DeSelect(_lastSelected);
                else NotifyDeSelect(_lastSelected);
            }

            SetSelected(index);
        }

        private void SetSelected(int index)
        {
            _lastSelected = index;
            bool unlocked = _assetInfoDTOs[index].Unlocked;
            _titleText.text = _assetInfoDTOs[index].AssetScript.Value;
            var buttonSelectionController = _buttonSelectionControllers[index];
            if (unlocked) NotifyUnLockedSelected(index);
            else NotifyLockedSelect(index);
            var normalizedPosition = _scrollRect.ScrollNormalizedPosition(buttonSelectionController.transform);
            Debug.Log(normalizedPosition);
        }

        private void DeSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(StatusSprite(false, _assetInfoDTOs[index].Unlocked));
            NotifyDeSelect(index);
        }

        private void NotifyUnLockedSelected(int index)
        {
            SetAssetEquippedOnCategory(_category, _assetInfoDTOs[index].AssetScript.guid);
            _buttonSelectionControllers[index].SetStatus(equippedSprite);
            _boatShopBase.OnUnlockedSelected(_playerAssetScriptComponent, _category, _assetInfoDTOs[index].AssetScript);
        }

        private void NotifyLockedSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(lockedSprite);
            _boatShopBase.OnLockedItemSelected(_playerAssetScriptComponent, _category,
                _assetInfoDTOs[index].AssetScript);
        }

        private void NotifyDeSelect(int index)
        {
            _buttonSelectionControllers[index].DeSelect();
            _boatShopBase.OnDeSelected(_playerAssetScriptComponent, _category, _assetInfoDTOs[index].AssetScript);
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
    }
}