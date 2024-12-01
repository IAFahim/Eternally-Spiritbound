﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Model.Focus.Runtime
{
    [Serializable]
    public class FocusReferences
    {
        public readonly Dictionary<AssetReferenceGameObject, GameObject> ActiveElements = new();
        [SerializeField] private Transform uiSillTransformPointPadded;
        [SerializeField] private Transform movingUITransformPoint;
        [SerializeField] private Transform movingUITransformPointPadded;
        [SerializeField] private GameObject currentGameObject;

        public Transform UISillTransformPointPadded => uiSillTransformPointPadded;

        public Transform MovingUITransformPoint => movingUITransformPoint;
        public Transform MovingUITransformPointPadded => movingUITransformPointPadded;

        public GameObject CurrentGameObject
        {
            get => currentGameObject;
            set => currentGameObject = value;
        }


        public void Clear()
        {
            currentGameObject = null;
            uiSillTransformPointPadded = null;
            movingUITransformPoint = null;
            foreach (var activeUiElement in ActiveElements)
            {
                Addressables.ReleaseInstance(activeUiElement.Value);
            }

            ActiveElements.Clear();
        }
    }
}