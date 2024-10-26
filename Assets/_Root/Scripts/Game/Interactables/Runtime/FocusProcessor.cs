using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public abstract class FocusProcessor : ScriptableObject
    {
        public abstract void SetFocus(FocusReferences focusReferences);

        protected void BuildCache(
            Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            params (AssetReferenceGameObject asset, Action<GameObject> Setup, Transform spawnTransfrom)[] cacheRequests)
        {
            List<AssetReferenceGameObject> keysToKeep = new List<AssetReferenceGameObject>();
            foreach (var (asset, setupCallback, spawnTransform) in cacheRequests)
            {
                keysToKeep.Add(asset);
                if (activeUiElementDictionary.TryGetValue(asset, out var element))
                {
                    setupCallback.Invoke(element);
                    element.SetActive(true);
                }
                else
                {
                    if (spawnTransform) Addressables.InstantiateAsync(asset, spawnTransform).Completed += Completed;
                    else Addressables.InstantiateAsync(asset).Completed += Completed;

                    void Completed(AsyncOperationHandle<GameObject> handle)
                    {
                        activeUiElementDictionary[asset] = handle.Result;
                        AfterSpanSetup(handle, setupCallback);
                        handle.Result.SetActive(true);
                    }
                }
            }

            var keysToRemove = activeUiElementDictionary.Keys.Except(keysToKeep).ToList();
            foreach (var key in keysToRemove)
            {
                activeUiElementDictionary[key].SetActive(false);
            }
        }

        private void AfterSpanSetup(AsyncOperationHandle<GameObject> operationHandle, Action<GameObject> setupCallBack)
        {
            if (operationHandle.Status != AsyncOperationStatus.Succeeded) return;
            setupCallBack(operationHandle.Result);
        }

        public abstract void OnFocusLost(GameObject targetGameObject);
    }
}