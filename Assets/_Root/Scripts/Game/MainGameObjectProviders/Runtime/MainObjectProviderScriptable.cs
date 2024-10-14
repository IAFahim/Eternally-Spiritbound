using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.UiLoaders.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainGameObjectInstance;
        public LayerMask layerMask;
        public CinemachineCamera virtualCamera;
        public Camera mainCamera;
        [FormerlySerializedAs("uISpawnPoint")] public Transform uISpawnPointTransform;

        [Header("Input Actions")] public InputActionReference moveAction;
        public InputActionReference accelerateAction;
        public GameObject lastFocusedGameObject;
        
        private IMoveInputConsumer _lastMoveInputConsumer;
        private IAccelerateInputConsumer _lastAccelerateInputConsumer;
        private Action<GameObject> _spawnedGameObjectCallBack;
        private IUIProvider _lastUiProvider;
        private readonly HashSet<GameObject> _spannedUIsHashset = new(); 

        public void SpawnMainGameObject(Camera camera,
            CinemachineCamera cinemachineCamera,
            Action<GameObject> gameObjectCallBack, Transform uiSpawnPoint)
        {
            mainCamera = camera;
            _spawnedGameObjectCallBack = gameObjectCallBack;
            virtualCamera = cinemachineCamera;
            uISpawnPointTransform = uiSpawnPoint;
            Addressables.InstantiateAsync(mainGameObjectAssetReference).Completed += OnCompletedInstantiate;
        }

        void OnCompletedInstantiate(AsyncOperationHandle<GameObject> handle)
        {
            mainGameObjectInstance = handle.Result;
            ProvideTo(mainGameObjectInstance, mainCamera, virtualCamera, uISpawnPointTransform);
            _spawnedGameObjectCallBack?.Invoke(mainGameObjectInstance);
            _spawnedGameObjectCallBack = null;
        }

        public void ProvideTo(GameObject gameObject, Camera camera, CinemachineCamera cinemachineCamera,
            Transform uiSpanPoint)
        {
            Clear();
            mainGameObjectInstance = gameObject;
            mainCamera = camera;
            virtualCamera = cinemachineCamera;
            uISpawnPointTransform = uiSpanPoint;
            AssignVirtualCamera(gameObject, camera, cinemachineCamera);
            AssignGameObject(gameObject);
            AssignMoveInput(gameObject);
            AssignAccelerateInput(gameObject);
            AssignUI(gameObject);
        }

        private void AssignUI(GameObject gameObject)
        {
            var uiProvider = gameObject.GetComponent<IUIProvider>();
            if (uiProvider == null) return;
            uiProvider.EnableUI(_spannedUIsHashset, uISpawnPointTransform, gameObject);
        }


        private void AssignVirtualCamera(GameObject gameObject, Camera camera, CinemachineCamera cinemachineCamera)
        {
            if (cinemachineCamera) cinemachineCamera.Follow = mainGameObjectInstance.transform;
            var mainCameraProvider = gameObject.GetComponent<IMainCameraProvider>();
            if (mainCameraProvider == null) return;
            mainCameraProvider.MainCamera = camera;
        }

        private void AssignGameObject(GameObject gameObject)
        {
            if (lastFocusedGameObject != null) lastFocusedGameObject.layer &= ~(layerMask);
            lastFocusedGameObject = gameObject;
            lastFocusedGameObject.layer |= 1 << layerMask;
        }

        private void AssignMoveInput(GameObject gameObject)
        {
            _lastMoveInputConsumer?.DisableMoveInput(moveAction);
            var inputConsumer = gameObject.GetComponent<IMoveInputConsumer>();
            if (inputConsumer == null) return;
            _lastMoveInputConsumer = inputConsumer;
            _lastMoveInputConsumer.EnableMoveInput(moveAction);
        }

        private void AssignAccelerateInput(GameObject gameObject)
        {
            _lastAccelerateInputConsumer?.DisableAccelerateInput(accelerateAction);
            var inputConsumer = gameObject.GetComponent<IAccelerateInputConsumer>();
            if (inputConsumer == null) return;
            _lastAccelerateInputConsumer = inputConsumer;
            _lastAccelerateInputConsumer.EnableAccelerateInput(accelerateAction);
        }

        private void Clear()
        {
            _lastAccelerateInputConsumer?.DisableAccelerateInput(accelerateAction);
            _lastMoveInputConsumer?.DisableMoveInput(moveAction);
            if (lastFocusedGameObject != null) lastFocusedGameObject.layer &= ~(1 << layerMask);
            if (lastFocusedGameObject != null) _lastUiProvider.DisableUI(lastFocusedGameObject);
            Forget();
        }

        public void Forget()
        {
            _spannedUIsHashset.Clear();
            _lastUiProvider = null; 
            _lastAccelerateInputConsumer = null;
            _lastMoveInputConsumer = null;
            lastFocusedGameObject = null;
        }
    }
}