using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.UiLoaders.Runtime
{
    public abstract class UIProviderScriptable : ScriptableObject, IUIProvider
    {
        protected GameObject TargetGameObject;
        protected Dictionary<AssetReferenceGameObject, GameObject> ActiveUiElementDictionary;

        public virtual void EnableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            Transform uISpawnPointTransform,
            GameObject targetGameObject)
        {
            TargetGameObject = targetGameObject;
            ActiveUiElementDictionary = activeUiElementDictionary;
        }

        protected void SetCache(
            Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary, Transform uiSpawnPointTransform,
            params CacheRequest[] cacheRequests)
        {
            foreach (var cacheRequest in cacheRequests)
            {
                if (activeUiElementDictionary.TryGetValue(cacheRequest.Asset, out GameObject uiElement))
                    cacheRequest.Hit.Invoke(uiElement);
                else
                    Addressables.InstantiateAsync(cacheRequest.Asset, uiSpawnPointTransform).Completed
                        += cacheRequest.Miss;
            }
        }

        // private void Setup(AsyncOperationHandle<GameObject> operationHandle)
        // {
        //     if (operationHandle.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         ActiveUiElementDictionary.Add(healthBarAsset, operationHandle.Result);
        //         SetupHealthBar(operationHandle.Result);
        //     }
        // }

        protected KeyValuePair<AssetReferenceGameObject, GameObject> InvalidateCacheEntry(
            AssetReferenceGameObject assetReferenceGameObject, GameObject uiElement, Action<GameObject> beforeClean)
        {
            beforeClean.Invoke(uiElement);
            return new KeyValuePair<AssetReferenceGameObject, GameObject>(assetReferenceGameObject, uiElement);
        }

        public abstract void DisableUI(Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            GameObject targetGameObject);
    }

    public struct CacheRequest
    {
        public readonly AssetReferenceGameObject Asset;
        public readonly Action<GameObject> Hit;
        public readonly Action<AsyncOperationHandle<GameObject>> Miss;

        public CacheRequest(AssetReferenceGameObject asset, Action<GameObject> onHit,
            Action<AsyncOperationHandle<GameObject>> onMiss)
        {
            Asset = asset;
            Hit = onHit;
            Miss = onMiss;
        }
    }
}