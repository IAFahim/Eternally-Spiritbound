using System;
using System.Collections.Generic;
using System.Threading;
using _Root.Scripts.Game.Infrastructures.Runtime.Shops;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using _Root.Scripts.Game.Utils.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Focus.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Linq;
using Soul.Interactables.Runtime;
using Soul.Pools.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProcessors.Runtime
{
    [CreateAssetMenu(fileName = "Shop Processor", menuName = "Scriptable/FocusProcessor/Shop")]
    public class ShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [FormerlySerializedAs("tabVerticalLayoutAsset")] [SerializeField]
        private AssetReferenceGameObject tabLayoutAsset;

        [SerializeField] private AssetReferenceGameObject scrollRectAsset;
        [SerializeField] private AssetReferenceGameObject buyButtonAsset;
        [SerializeField] private AssetReferenceGameObject shopCloseButtonAsset;
        [SerializeField] private AssetReferenceGameObject buttonSelectionControllerAsset;
        [SerializeField] private AssetReferenceGameObject tabButtonControllerAsset;


        [SerializeField] private FocusManagerScript focusManagerScript;


        [SerializeField] private StatusSprite lockedStatusSprite;

        [SerializeField] private StatusSprite equippedStatusSprite;

        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite equippedSprite;

        private Button _closeButton;
        [SerializeField] private AssetScriptOwnsAssetsScriptLink assetScriptOwnsAssetsScriptLink;


        private List<AssetScript> _unlockedAssets;
        private ButtonSelectionController[] _buttonSelectionControllers;
        private PriceButtonController _buyButtonSelectionController;
        private AssetScriptReferenceComponent _playerAssetScriptReferenceComponent;
        private ScrollRect _scrollRect;
        private ShopBase _shopBase;
        private HorizontalLayoutGroup _tabLayoutGroup;
        private TabButtonController[] _tabButtonControllers;

        private AssetInfoDto[] _assetInfoDTOs;
        private TMP_Text _titleText;
        private int _lastSelectedIndexOnThisTab;
        private int _selectedTabIndex;


        private string GetSelectedTabTitleString() => PlayerPrefs.GetString(name, string.Empty);
        private string StoreSelectedTabString(string category) => PlayerPrefs.GetString(name, category);


        public override void SetFocus(FocusReferences focusReferences, CancellationToken token)
        {
            TargetGameObject = focusReferences.CurrentGameObject;
            _shopBase = TargetGameObject.GetComponent<ShopBase>();
            _playerAssetScriptReferenceComponent =
                focusManagerScript.mainObject.GetComponent<AssetScriptReferenceComponent>();

            BuildCache(
                focusReferences.ActiveElements, BeforeActive, token,
                (cinemachineAsset, SetupCinemachine, null),
                (tabLayoutAsset, SetupTabButton, focusReferences.UISillTransformPointPadded),
                (shopCloseButtonAsset, SetupCloseButton, focusReferences.UISillTransformPointPadded),
                (scrollRectAsset, SetupScrollRect, focusReferences.MovingUITransformPointPadded),
                (buyButtonAsset, SetupBuyButton, focusReferences.UISillTransformPointPadded)
            ).Forget();
        }


        private void SetupTabButton(GameObject obj)
        {
            _tabLayoutGroup = obj.GetComponent<HorizontalLayoutGroup>();
        }

        private async UniTask SetupTabButtonAsync()
        {
            var assetCategories = _shopBase.GetAssetCategories();
            _tabButtonControllers = new TabButtonController[assetCategories.Length];
            var selectedTabTitleString = GetSelectedTabTitleString();
            _selectedTabIndex = 0;
            for (var i = 0; i < assetCategories.Length; i++)
            {
                var assetCategory = assetCategories[i];
                _tabButtonControllers[i] = await SharedAssetPoolInactive.RequestAsync<TabButtonController>(
                    tabButtonControllerAsset, _tabLayoutGroup.transform
                );

                _tabButtonControllers[i].Init(i, assetCategory.title, assetCategory.icon, TabSelectionClick);
                if (assetCategory.title == selectedTabTitleString) _selectedTabIndex = i;
                else _tabButtonControllers[i].SetSelected(false);
            }

            TabSelectionClick(_selectedTabIndex);
        }

        private void TabSelectionClick(int tabIndex)
        {
            var selectedCategory = _shopBase.GetAssetCategories()[tabIndex];
            if (tabIndex == _selectedTabIndex) return;
            StoreSelectedTabString(selectedCategory.title);
        }

        private void SetupScrollRect(GameObject gameObject)
        {
            _scrollRect = gameObject.GetComponent<ScrollRect>();
            _titleText = gameObject.GetComponentInChildren<TMP_Text>();
        }

        private void BeforeActive()
        {
            BeforeActiveAsync().Forget();
        }

        private async UniTaskVoid BeforeActiveAsync()
        {
            Debug.Log(_selectedTabIndex);
            await SetupTabButtonAsync();
            await InstantiateCategory(_shopBase.GetAssetCategories()[_selectedTabIndex]);

            foreach (var tabButtonController in _tabButtonControllers)
            {
                tabButtonController.gameObject.SetActive(true);
            }

            foreach (var buttonSelectionController in _buttonSelectionControllers)
            {
                buttonSelectionController.gameObject.SetActive(true);
            }
        }


        private async UniTask InstantiateCategory(AssetCategory assetCategory)
        {
            bool linkExist =
                assetScriptOwnsAssetsScriptLink.TryGetValue(_playerAssetScriptReferenceComponent.assetScriptReference,
                    out _unlockedAssets);
            _assetInfoDTOs = CreateInfoDto(assetCategory.assets, linkExist);
            Array.Sort(_assetInfoDTOs);
            await PopulatePool(_scrollRect, assetCategory, _assetInfoDTOs);
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


        private async UniTask PopulatePool(ScrollRect scrollRect, AssetCategory assetCategory,
            AssetInfoDto[] assetInfoDTOs)
        {
            _buttonSelectionControllers = new ButtonSelectionController[assetCategory.assets.Count];
            var scrollContentTransform = scrollRect.content.transform;
            for (var i = 0; i < assetInfoDTOs.Length; i++)
            {
                (ButtonSelectionController buttonSelectionController, bool isEquipped) =
                    await CreateController(i, scrollContentTransform, assetInfoDTOs[i]);
                _buttonSelectionControllers[i] = buttonSelectionController;
                if (isEquipped) _lastSelectedIndexOnThisTab = i;
            }

            SelectAssetInCurrentTab(_lastSelectedIndexOnThisTab);
        }


        private async UniTask<(ButtonSelectionController, bool)> CreateController(int index,
            Transform scrollContentTransform,
            AssetInfoDto assetInfoDto)
        {
            var buttonSelectionController = await SharedAssetPoolInactive
                .RequestAsync<ButtonSelectionController>(buttonSelectionControllerAsset, scrollContentTransform);
            buttonSelectionController.transform.SetSiblingIndex(index);
            var isEquipped = _shopBase.equippedItemGuid == assetInfoDto.AssetScript.Guid;
            buttonSelectionController.Initialize(
                index, isEquipped,
                assetInfoDto.AssetScript.Icon,
                GetSpriteStats(isEquipped, assetInfoDto.Unlocked),
                SelectAssetInCurrentTab
            );
            return (buttonSelectionController, isEquipped);
        }


        private void SelectAssetInCurrentTab(int index)
        {
            if (_lastSelectedIndexOnThisTab != index)
            {
                if (_assetInfoDTOs[index].Unlocked) DeSelect(_lastSelectedIndexOnThisTab);
                else NotifyDeSelect(_lastSelectedIndexOnThisTab);
            }

            SetSelected(index);
        }

        private void SetSelected(int index)
        {
            _lastSelectedIndexOnThisTab = index;
            bool unlocked = _assetInfoDTOs[index].Unlocked;
            _titleText.text = _assetInfoDTOs[index].AssetScript.Value;
            var buttonSelectionController = _buttonSelectionControllers[index];
            if (unlocked) NotifyUnLockedSelected(index);
            else NotifyLockedSelect(index);
            SetupBuyButton(index, unlocked);

            var normalizedPosition = _scrollRect.ScrollNormalizedPosition(buttonSelectionController.transform);
            Debug.Log(normalizedPosition);
        }

        private void SetupBuyButton(GameObject gameObject)
        {
            _buyButtonSelectionController = gameObject.GetComponent<PriceButtonController>();
        }

        private void SetupBuyButton(int index, bool unlocked)
        {
            var assetScript = _assetInfoDTOs[index].AssetScript;
            var hasEnough = _shopBase.HasEnough(_playerAssetScriptReferenceComponent, assetScript, out var assetPrice);
            _buyButtonSelectionController.Initialize(
                assetPrice.asset.Icon,
                assetPrice.price,
                "Buy",
                hasEnough,
                OnBuyButtonPressed
            );
            _buyButtonSelectionController.gameObject.SetActive(!unlocked);
        }


        private void OnBuyButtonPressed()
        {
            var assetScript = _assetInfoDTOs[_lastSelectedIndexOnThisTab].AssetScript;
            var buySuccess = _shopBase.OnTryBuyButtonClick(_playerAssetScriptReferenceComponent, _selectedTabIndex,
                assetScript,
                out var message);
            Debug.Log($"[{buySuccess}]: {message}");
        }


        private void DeSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatusImage(GetSpriteStats(false, _assetInfoDTOs[index].Unlocked));
            NotifyDeSelect(index);
        }

        private void NotifyUnLockedSelected(int index)
        {
            _shopBase.equippedItemGuid = _assetInfoDTOs[index].AssetScript.Guid;
            _buttonSelectionControllers[index].SetStatusImage(equippedStatusSprite);
            _shopBase.OnUnlockedSelected(_playerAssetScriptReferenceComponent, _selectedTabIndex,
                _assetInfoDTOs[index].AssetScript);
        }

        private void NotifyLockedSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatusImage(lockedStatusSprite);
            _shopBase.OnLockedItemSelected(_playerAssetScriptReferenceComponent, _selectedTabIndex,
                _assetInfoDTOs[index].AssetScript);
        }

        private void NotifyDeSelect(int index)
        {
            _buttonSelectionControllers[index].DeSelect();
            _shopBase.OnDeSelected(_playerAssetScriptReferenceComponent, _selectedTabIndex,
                _assetInfoDTOs[index].AssetScript);
        }

        private StatusSprite GetSpriteStats(bool isEquipped, bool unlocked)
        {
            if (!unlocked) return lockedStatusSprite;
            if (isEquipped) return equippedStatusSprite;
            return null;
        }


        private void SetupCloseButton(GameObject gameObject)
        {
            focusManagerScript.PeekFocus().OnPushFocus += OnFocusLost;
            _closeButton = gameObject.GetComponent<Button>();
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            _closeButton.onClick.AddListener(TryPopAndActiveLast);
        }

        private void CleanAssetSelection()
        {
            foreach (var buttonSelectionController in _buttonSelectionControllers)
            {
                SharedAssetPoolInactive.Return(
                    buttonSelectionControllerAsset,
                    buttonSelectionController.gameObject
                );
            }

            SharedAssetPoolInactive.Return(scrollRectAsset, _scrollRect.gameObject);
        }

        private void CleanTabs()
        {
            foreach (var tabButtonController in _tabButtonControllers)
            {
                SharedAssetPoolInactive.Return(tabButtonControllerAsset, tabButtonController.gameObject);
            }

            SharedAssetPoolInactive.Return(tabLayoutAsset, _tabLayoutGroup.gameObject);
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
            CleanTabs();
            CleanAssetSelection();
            SharedAssetPoolInactive.Return(shopCloseButtonAsset, _closeButton.gameObject);
            SharedAssetPoolInactive.Return(buyButtonAsset, _buyButtonSelectionController.gameObject);
            _shopBase.OnExit(focusManagerScript.mainObject.GetComponent<IInteractorEntryPoint>());
        }

        private void TryPopAndActiveLast() => focusManagerScript.PopFocus();
    }
}