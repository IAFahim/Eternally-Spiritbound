﻿using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class FocusManagerInitialization : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        [FormerlySerializedAs("focusManager")] public FocusManagerScript focusManagerScript;
        public FocusReferences focusReferences;

        private void Awake()
        {
            focusManagerScript.Initialize(mainCamera, focusReferences);
            if (mainGameObjectInstance == null)
                focusManagerScript.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                focusManagerScript.PushFocus(mainGameObjectInstance.GetComponent<IFocusEntryPoint>());
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            focusManagerScript.Clear();
        }
    }
}