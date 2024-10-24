﻿using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public class FocusManagerComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        [FormerlySerializedAs("mainStackScriptable")] [FormerlySerializedAs("mainProviderScriptable")] public FocusScriptable focusScriptable;
        [FormerlySerializedAs("transformReferences")] public FocusReferences focusReferences;

        private void Awake()
        {
            focusScriptable.Initialize(mainCamera, focusReferences);
            if (mainGameObjectInstance == null)
                focusScriptable.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                focusScriptable.Push(new FocusInfo(mainGameObjectInstance, true, null));
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            focusScriptable.Forget();
        }
    }
}