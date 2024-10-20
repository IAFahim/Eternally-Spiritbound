using System;
using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    [CreateAssetMenu(fileName = "BoatShopFocusProvider", menuName = "Scriptable/FocusProviders/BoatShop")]
    public class BoatShopFocusConsumerScriptable : FocusConsumerCinemachineScriptable
    {

        public override void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences, GameObject targetGameObject)
        {
            TargetGameObject = targetGameObject;
            BuildCache(
                activeElements,
                (cinemachineAsset, SetupCinemachine, null)
            );
        }
    }
}