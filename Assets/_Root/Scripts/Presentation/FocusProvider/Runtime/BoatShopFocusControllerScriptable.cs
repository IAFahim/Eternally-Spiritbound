﻿using System.Collections.Generic;
using _Root.Scripts.Game.FocusProvider.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "BoatShopFocusProvider", menuName = "Scriptable/FocusProviders/BoatShop")]
    public class BoatShopFocusControllerScriptable : FocusControllerCinemachineScriptable
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
            FocusScriptable.Instance.Peek().OnPop += Pop;
            _closeButton = gameObject.GetComponent<Button>();
            _closeButton.onClick.AddListener(TryPopAndActiveLast);
        }

        private void Pop(FocusScriptable obj)
        {
            OnFocusLost(TargetGameObject);
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            base.OnFocusLost(targetGameObject);
            _closeButton.onClick.RemoveListener(TryPopAndActiveLast);
        }

        private void TryPopAndActiveLast()
        {
            FocusScriptable.Instance.TryPopAndActiveLast();
        }
    }
}