using System;
using _Root.Scripts.Game.Inputs.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainGameObjectInstance;
        public LayerMask layerMask;
        public CinemachineCamera virtualCamera;
        public Camera mainCamera;

        [Header("Input Actions")] public InputActionReference moveAction;
        public InputActionReference accelerateAction;

        public GameObject lastFocusedGameObject;
        private IMoveInputConsumer lastMoveInputConsumer;
        private IAccelerateInputConsumer lastAccelerateInputConsumer;
        private Action<GameObject> spawnedGameObjectCallBack;

        public void SpawnMainGameObject(
            Camera camera,
            CinemachineCamera cinemachineCamera,
            Action<GameObject> gameObjectCallBack)
        {
            mainCamera = camera;
            spawnedGameObjectCallBack = gameObjectCallBack;
            virtualCamera = cinemachineCamera;
            Addressables.InstantiateAsync(mainGameObjectAssetReference).Completed += OnCompletedInstantiate;
        }

        void OnCompletedInstantiate(AsyncOperationHandle<GameObject> handle)
        {
            mainGameObjectInstance = handle.Result;
            AssignVirtualCamera(mainGameObjectInstance, mainCamera, virtualCamera);
            ProvideTo(mainGameObjectInstance, mainCamera, virtualCamera);
            spawnedGameObjectCallBack?.Invoke(mainGameObjectInstance);
            spawnedGameObjectCallBack = null;
        }

        private void AssignVirtualCamera(GameObject gameObject, Camera camera, CinemachineCamera cinemachineCamera)
        {
            if (cinemachineCamera) cinemachineCamera.Follow = mainGameObjectInstance.transform;
            var mainCameraProvider = gameObject.GetComponent<IMainCameraProvider>();
            if (mainCameraProvider == null) return;
            mainCameraProvider.MainCamera = camera;
        }

        public void ProvideTo(GameObject gameObject, Camera camera, CinemachineCamera cinemachineCamera)
        {
            mainGameObjectInstance = gameObject;
            mainCamera = camera;
            AssignVirtualCamera(gameObject, camera, cinemachineCamera);
            AssignGameObject(gameObject);
            AssignMoveInput(gameObject);
            AssignAccelerateInput(gameObject);
        }


        private void AssignGameObject(GameObject gameObject)
        {
            if (lastFocusedGameObject != null) lastFocusedGameObject.layer &= ~(layerMask);
            lastFocusedGameObject = gameObject;
            lastFocusedGameObject.layer |= 1 << layerMask;
        }

        private void AssignMoveInput(GameObject gameObject)
        {
            lastMoveInputConsumer?.DisableMoveInput(moveAction);
            var inputConsumer = gameObject.GetComponent<IMoveInputConsumer>();
            if (inputConsumer == null) return;
            lastMoveInputConsumer = inputConsumer;
            lastMoveInputConsumer.EnableMoveInput(moveAction);
        }

        private void AssignAccelerateInput(GameObject gameObject)
        {
            lastAccelerateInputConsumer?.DisableAccelerateInput(accelerateAction);
            var inputConsumer = gameObject.GetComponent<IAccelerateInputConsumer>();
            if (inputConsumer == null) return;
            lastAccelerateInputConsumer = inputConsumer;
            lastAccelerateInputConsumer.EnableAccelerateInput(accelerateAction);
        }

        public void OnDisable()
        {
            if (lastAccelerateInputConsumer != null)
            {
                lastAccelerateInputConsumer.DisableAccelerateInput(accelerateAction);
                lastAccelerateInputConsumer = null;
            }

            if (lastMoveInputConsumer != null)
            {
                lastMoveInputConsumer.DisableMoveInput(moveAction);
                lastMoveInputConsumer = null;
            }

            if (lastFocusedGameObject != null)
            {
                lastFocusedGameObject.layer &= ~(1 << layerMask);
                lastFocusedGameObject = null;
            }
        }
    }
}