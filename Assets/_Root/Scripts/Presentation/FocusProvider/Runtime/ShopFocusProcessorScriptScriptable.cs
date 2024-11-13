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
using UnityEngine.Serialization;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Shop Processor", menuName = "Scriptable/FocusProcessor/Shop")]
    public class ShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [FormerlySerializedAs("boatScrollRectAsset")] [SerializeField]
        private AssetReferenceGameObject scrollRectAsset;

        [SerializeField] private AssetReferenceGameObject buyButtonAsset;

        [FormerlySerializedAs("boatShopCloseButtonAsset")] [SerializeField]
        private AssetReferenceGameObject shopCloseButtonAsset;

        [FormerlySerializedAs("focusManager")] [SerializeField]
        private FocusManagerScript focusManagerScript;

        [SerializeField] private AssetPriceLink assetPriceLink;
        [SerializeField] private AssetReferenceGameObject buttonSelectionControllerAsset;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite equippedSprite;

        private Button _closeButton;
        [SerializeField] private AssetOwnsAssetsLink assetOwnsAssetsLink;


        private List<AssetScript> _unlockedAssets;
        private ButtonSelectionController[] _buttonSelectionControllers;
        private BuyButtonController _buybuttonSelectionController;
        private AssetScriptReferenceComponent _playerAssetScriptReferenceComponent;
        private ScrollRect _scrollRect;
        private ShopBase _shopBase;
        private string _category;
        private Transform _stillCanvasTransformPoint;

        private AssetInfoDto[] _assetInfoDTOs;
        private TMP_Text _titleText;
        private int _lastSelected;


        private string GetSelectedCategory() => PlayerPrefs.GetString(value, string.Empty);
        private string SetSelectedCategory(string category) => PlayerPrefs.GetString(value, category);

        private void SetAssetEquippedOnCategory(string category, string guid) =>
            PlayerPrefs.GetString(focusManagerScript.mainObject.name + category + value, guid);

        private string GetAssetEquippedCategorySelected(string category) =>
            PlayerPrefs.GetString(PlayerEquippedPerCategoryLookUpKey(category), string.Empty);

        private string PlayerEquippedPerCategoryLookUpKey(string category) =>
            focusManagerScript.mainObject.name + category + value;

        public override void SetFocus(FocusReferences focusReferences)
        {
            _stillCanvasTransformPoint = focusReferences.StillCanvasTransformPoint;
            TargetGameObject = focusReferences.CurrentGameObject;
            BuildCache(
                focusReferences.ActiveElements,
                (cinemachineAsset, SetupCinemachine, null),
                (shopCloseButtonAsset, SetupCloseButton, focusReferences.StillCanvasTransformPoint),
                (scrollRectAsset, SetupScrollRect, focusReferences.StillCanvasTransformPoint)
            );
        }

        private void SetupScrollRect(GameObject gameObject)
        {
            _lastSelected = 0;
            _scrollRect = gameObject.GetComponent<ScrollRect>();
            _titleText = gameObject.GetComponentInChildren<TMP_Text>();
            InstantiateShopBase();
        }


        private void InstantiateShopBase()
        {
            _playerAssetScriptReferenceComponent =
                focusManagerScript.mainObject.GetComponent<AssetScriptReferenceComponent>();
            _shopBase = TargetGameObject.GetComponent<ShopBase>();
            var assetCategories = _shopBase.assetCategories;
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
                assetOwnsAssetsLink.TryGetValue(_playerAssetScriptReferenceComponent.assetScriptReference,
                    out _unlockedAssets);
            _assetInfoDTOs = CreateInfoDto(assetCategory.assets, linkExist);
            Array.Sort(_assetInfoDTOs);
            PopulatePool(_scrollRect, assetCategory, _assetInfoDTOs);
        }

        private AssetInfoDto[] CreateInfoDto(List<AssetScript> assetScripts, bool linkExist)
        {
            var assetInfoDtos = new AssetInfoDto[assetScripts.Count];
            for (var i = 0; i < assetScripts.Count; i++)
            {
                var assetScript = assetScripts[i];
                var unlocked = linkExist && _unlockedAssets.Any(asset => asset.Guid == assetScript.Guid);
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
            SetupBuyButton(_lastSelected, true);
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
            isEquipped = equippedAssetGuid == assetInfoDto.AssetScript.Guid;
            buttonSelectionController.Initialize(
                index, isEquipped,
                assetInfoDto.AssetScript.Icon,
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

        private void SetupBuyButton(int index, bool spawn)
        {
            if (spawn)
            {
                _buybuttonSelectionController = SharedAssetReferencePoolInactive
                    .Request<BuyButtonController>(buyButtonAsset, _stillCanvasTransformPoint, true);
            }

            var assetScript = _assetInfoDTOs[index].AssetScript;
            var hasEnough = HasEnoguh(assetScript, out var price);
            _buybuttonSelectionController.Initialize(assetScript.Icon, price, hasEnough, OnBuyButtonPressed);
        }

        private void OnBuyButtonPressed()
        {
            var assetScript = _assetInfoDTOs[_lastSelected].AssetScript;
            var buySuccess = _shopBase.OnTryBuyButtonClick(_playerAssetScriptReferenceComponent, _category, assetScript,
                out var message);
            Debug.Log($"{buySuccess}: {message}");
        }

        public bool HasEnoguh(AssetScript assetScript, out int price)
        {
            if (!assetPriceLink.TryGetValue(assetScript, out var assetPrice))
            {
                price = 0;
                return true;
            }
            else
            {
                // _playerAssetScriptReferenceComponent.
            }

            price = 0;
            return true;
        }


        private void DeSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(StatusSprite(false, _assetInfoDTOs[index].Unlocked));
            NotifyDeSelect(index);
        }

        private void NotifyUnLockedSelected(int index)
        {
            SetAssetEquippedOnCategory(_category, _assetInfoDTOs[index].AssetScript.Guid);
            _buttonSelectionControllers[index].SetStatus(equippedSprite);
            _shopBase.OnUnlockedSelected(_playerAssetScriptReferenceComponent, _category,
                _assetInfoDTOs[index].AssetScript);
        }

        private void NotifyLockedSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(lockedSprite);
            _shopBase.OnLockedItemSelected(_playerAssetScriptReferenceComponent, _category,
                _assetInfoDTOs[index].AssetScript);
        }

        private void NotifyDeSelect(int index)
        {
            _buttonSelectionControllers[index].DeSelect();
            _shopBase.OnDeSelected(_playerAssetScriptReferenceComponent, _category,
                _assetInfoDTOs[index].AssetScript);
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

            _shopBase.OnExit(null);
        }

        private void SetupCloseButton(GameObject gameObject)
        {
            focusManagerScript.PeekFocus().OnPushFocus += OnFocusLost;
            _closeButton = gameObject.GetComponent<Button>();
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            _closeButton.onClick.AddListener(TryPopAndActiveLast);
        }

        private void TryPopAndActiveLast() => focusManagerScript.PopFocus();
    }
}