using System;
using System.Collections.Generic;
using System.Threading;
using _Root.Scripts.Model.Focus.Runtime;
using Cysharp.Threading.Tasks;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime.Focus
{
    public abstract class FocusProcessorScript : ScriptableObject
    {
        public abstract void SetFocus(FocusReferences focusReferences, CancellationToken cancellationToken);

        protected async UniTask BuildCache(
            Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary, CancellationToken token,
            params (AssetReferenceGameObject asset, Action<GameObject> Setup, Transform spawnTransfrom)[] cacheRequests)
        {
            LoadTasks(activeUiElementDictionary, token, cacheRequests,
                out var loadTasksPendingSetup,
                out var keysToKeep,
                out var pendingSetups
            );
            DisableUnusedElements(activeUiElementDictionary, keysToKeep);
            var gameObjects = await UniTask.WhenAll(loadTasksPendingSetup);
            int index = 0;
            for (var i = 0; i < pendingSetups.Count; i++)
            {
                var activePendingSetup = pendingSetups[i];
                if (activePendingSetup.Awaited)
                {
                    activeUiElementDictionary.Add(cacheRequests[i].asset, gameObjects[index]);
                    activePendingSetup.GameObject = gameObjects[index];
                    index++;
                }

                cacheRequests[i].Setup(activePendingSetup.GameObject);
            }
            
            foreach (var pendingSetup in pendingSetups) pendingSetup.GameObject.SetActive(true);
        }

        private static void DisableUnusedElements(
            Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            HashSet<AssetReferenceGameObject> keysToKeep)
        {
            foreach (var (assetReferenceGameObject, gameObject) in activeUiElementDictionary)
            {
                if (!keysToKeep.Contains(assetReferenceGameObject)) gameObject.SetActive(false);
            }
        }

        private void LoadTasks(
            Dictionary<AssetReferenceGameObject, GameObject> activeUiElementDictionary,
            CancellationToken token,
            (AssetReferenceGameObject asset, Action<GameObject> Setup, Transform spawnTransfrom)[] cacheRequests,
            out List<UniTask<GameObject>> loadTasks,
            out HashSet<AssetReferenceGameObject> keysToKeep,
            out List<PendingSetup> pendingSetup)
        {
            loadTasks = new();
            keysToKeep = new();
            pendingSetup = new();
            foreach (var (asset, setupCallback, spawnTransform) in cacheRequests)
            {
                keysToKeep.Add(asset);
                if (activeUiElementDictionary.TryGetValue(asset, out var element))
                {
                    pendingSetup.Add(new(false, element));
                }
                else
                {
                    loadTasks.Add(spawnTransform
                        ? asset.InstantiateAsync(spawnTransform).ToUniTask(cancellationToken: token)
                        : asset.InstantiateAsync().ToUniTask(cancellationToken: token)
                    );
                    pendingSetup.Add(new(true, null));
                }
            }
        }

        public abstract void OnFocusLost(GameObject targetGameObject);
    }

    public class PendingSetup
    {
        public bool Awaited;
        public GameObject GameObject;

        public PendingSetup(bool awaited, GameObject gameObject)
        {
            Awaited = awaited;
            GameObject = gameObject;
        }
    }
}