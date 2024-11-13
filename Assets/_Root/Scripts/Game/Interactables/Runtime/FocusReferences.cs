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
        [SerializeField] private Transform stillCanvasTransformPoint;
        [SerializeField]  private Transform movingCanvasTransformPoint;
        [SerializeField] private GameObject currentGameObject;

        public Transform StillCanvasTransformPoint => stillCanvasTransformPoint;

        public Transform MovingCanvasTransformPoint => movingCanvasTransformPoint;

        public GameObject CurrentGameObject
        {
            get => currentGameObject;
            internal set => currentGameObject = value;
        }

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