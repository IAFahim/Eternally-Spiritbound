using System;
using _Root.Scripts.Game.Infrastructures.Runtime.Shops;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Utils.Runtime;
using _Root.Scripts.Model.Boats.Runtime;
using _Root.Scripts.Presentation.Containers.Runtime;
using Soul.Pools.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "Boat Shop Processor", menuName = "Scriptable/FocusProcessor/Boat Shop")]
    public class BoatShopFocusProcessorScriptScriptable : FocusProcessorScriptCinemachineScriptable
    {
        [FormerlySerializedAs("boatShopCloseButton")] [SerializeField] private AssetReferenceGameObject boatShopCloseButtonAsset;
        [FormerlySerializedAs("boatScrollRect")] [SerializeField] private AssetReferenceGameObject boatScrollRectAsset;
        [SerializeField] private FocusManagerScript focusManager;
        [SerializeField] private AssetReferenceGameObject buttonSelectionControllerAsset;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite equippedSprite;

        [SerializeField] private BoatVehicleAsset[] boatVehicleAssets;
        [SerializeField] private ScriptablePool scriptablePool;
        private Button _closeButton;

        [SerializeField] private string equippedBoatName;
        [SerializeField] private string[] unlockedBoatNames;
        [SerializeField] private ButtonSelectionController[] _buttonSelectionControllers;

        private ScrollRect _scrollRect;
        private ShopBase _shopBase;
        private BoatInfoDto[] _boatInfoDTOs;
        private TMP_Text _titleText;  
        private int _lastSelected;

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
                    Index = i,
                    Unlocked = Array.Exists(unlockedBoatNames, s => s == boatVehicle.Value),
                    BoatVehicleAsset = boatVehicle
                };
            }

            return boatInfoDTOs;
        }

        private int Comparison(BoatInfoDto a, BoatInfoDto b)
        {
            //show unlocked first but keep the order
            if (a.Unlocked && !b.Unlocked) return -1;
            if (!a.Unlocked && b.Unlocked) return 1;
            return a.Index.CompareTo(b.Index);
        }

        private void PopulatePool(ScrollRect scrollRect, BoatInfoDto[] boatInfoDTOs)
        {
            _buttonSelectionControllers = new ButtonSelectionController[boatVehicleAssets.Length];
            var scrollContentTransform = scrollRect.content.transform;
            for (var i = 0; i < boatInfoDTOs.Length; i++)
            {
                _buttonSelectionControllers[i] = CreateController(i, scrollContentTransform, boatInfoDTOs[i],
                    out var isEquipped);
                if (isEquipped) _lastSelected = i;
            }

            Select(_lastSelected);
        }

        private ButtonSelectionController CreateController(int index,
            Transform scrollContentTransform,
            BoatInfoDto boatInfoDto,
            out bool isEquipped)
        {
            var buttonSelectionController = scriptablePool
                .Request(buttonSelectionControllerAsset, scrollContentTransform)
                .GetComponent<ButtonSelectionController>();

            isEquipped = equippedBoatName == boatInfoDto.BoatVehicleAsset.Value;
            buttonSelectionController.Initialize(
                index,
                boatInfoDto.BoatVehicleAsset.icon,
                StatusSprite(isEquipped, boatInfoDto.Unlocked),
                Select
            );
            return buttonSelectionController;
        }


        private void Select(int index)
        {
            if (_lastSelected != index && _boatInfoDTOs[index].Unlocked) UnSelect(_lastSelected);
            SetSelected(index);
        }

        private void SetSelected(int index)
        {
            bool unlocked = _boatInfoDTOs[index].Unlocked;
            if (unlocked) _lastSelected = index;
            _titleText.text = _boatInfoDTOs[index].BoatVehicleAsset.Value;
            var buttonSelectionController = _buttonSelectionControllers[index];
            buttonSelectionController.SetStatus(StatusSprite(unlocked, unlocked));
            var normalizedPosition = _scrollRect.ScrollNormalizedPosition(buttonSelectionController.transform);
            Debug.Log(normalizedPosition);
        }

        private void UnSelect(int index)
        {
            _buttonSelectionControllers[index].SetStatus(StatusSprite(false, _boatInfoDTOs[index].Unlocked));
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

        private struct BoatInfoDto
        {
            public int Index;
            public bool Unlocked;
            public BoatVehicleAsset BoatVehicleAsset;

            public override string ToString()
            {
                return
                    $"{nameof(Index)}: {Index}, {nameof(Unlocked)}: {Unlocked}";
            }
        }
    }
}