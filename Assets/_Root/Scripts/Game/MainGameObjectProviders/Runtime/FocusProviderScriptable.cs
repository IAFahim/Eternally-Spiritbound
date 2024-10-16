using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public abstract class FocusProviderScriptable : ScriptableObject, IFocusProvider
    {
        public abstract void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            Transform uISpawnPointTransform, GameObject targetGameObject, Action returnFocusCallBack);

        protected void SetCache(
            Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary, Transform uiSpawnTransform,
            params (AssetReferenceGameObject asset, Action<GameObject> Setup, bool isUI)[] cacheRequests)
        {
            List<AssetReferenceGameObject> keysToKeep = new List<AssetReferenceGameObject>();
            foreach (var (asset, setupCallback, isUI) in cacheRequests)
            {
                keysToKeep.Add(asset);
                if (activeUiElementDictionary.TryGetValue(asset, out var uiElement)) setupCallback.Invoke(uiElement);
                else
                {
                    if (isUI)
                    {
                        Addressables.InstantiateAsync(asset, uiSpawnTransform).Completed +=
                            handle => Setup(handle, setupCallback);
                    }
                    else Addressables.InstantiateAsync(asset).Completed += handle => Setup(handle, setupCallback);
                }
            }

            var keysToRemove = activeUiElementDictionary.Keys.Except(keysToKeep).ToList();
            foreach (var key in keysToRemove)
            {
                Addressables.ReleaseInstance(activeUiElementDictionary[key]);
                activeUiElementDictionary.Remove(key);
            }
        }

        private void Setup(AsyncOperationHandle<GameObject> operationHandle, Action<GameObject> setupCallBack)
        {
            if (operationHandle.Status != AsyncOperationStatus.Succeeded) return;
            setupCallBack(operationHandle.Result);
        }

        public abstract void OnFocusLost(GameObject targetGameObject);
    }
}