using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.UiLoaders.Runtime
{
    public interface IUIProvider
    {
        public void EnableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            Transform uISpawnPointTransform, GameObject targetGameObject);

        public void DisableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            GameObject targetGameObject);
    }
}