using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.FocusProvider.Runtime
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