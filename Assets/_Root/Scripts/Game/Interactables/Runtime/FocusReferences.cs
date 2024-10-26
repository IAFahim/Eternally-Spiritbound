using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    [Serializable]
    public class FocusReferences
    {
        public readonly Dictionary<AssetReferenceGameObject, GameObject> ActiveElements = new();
        public Transform stillCanvasTransformPoint;
        public Transform movingCanvasTransformPoint;
        public GameObject currentGameObject;

        public void Clear()
        {
            currentGameObject = null;
            stillCanvasTransformPoint = null;
            movingCanvasTransformPoint = null;
            foreach (var activeUiElement in ActiveElements)
            {
                Addressables.ReleaseInstance(activeUiElement.Value);
            }

            ActiveElements.Clear();
        }
    }
}