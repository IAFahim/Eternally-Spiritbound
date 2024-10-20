using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainProviderScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        [Header("Input Actions")] public InputActionReference moveAction;

        private TransformReferences _transformReferences;
        private Action<GameObject> _spawnedGameObjectCallBack;
        private readonly Dictionary<AssetReferenceGameObject, GameObject> _activeElements = new();

        // Stack to store the focused GameObjects
        private readonly Stack<GameObject> _focusStack = new();

        public void Initialize(Camera camera, TransformReferences transformReferences)
        {
            mainCamera = camera;
            _transformReferences = transformReferences;
        }

        public void SpawnMainGameObject(Action<GameObject> gameObjectCallBack)
        {
            _spawnedGameObjectCallBack = gameObjectCallBack;
            Addressables.InstantiateAsync(mainGameObjectAssetReference).Completed += OnCompletedInstantiate;
        }

        void OnCompletedInstantiate(AsyncOperationHandle<GameObject> handle)
        {
            mainGameObjectInstance = handle.Result;
            ProvideTo(mainGameObjectInstance);
            _spawnedGameObjectCallBack?.Invoke(mainGameObjectInstance);
            _spawnedGameObjectCallBack = null;
        }

        public void ProvideTo(GameObject gameObject)
        {
            if (mainGameObjectInstance != null)
            {
                UnLink(mainGameObjectInstance);
                _focusStack.Push(mainGameObjectInstance);
            }

            Setup(gameObject);
        }

        private void Setup(GameObject gameObject)
        {
            mainGameObjectInstance = gameObject;
            AssignCamera(gameObject, mainCamera);
            AssignMoveInput(gameObject);
            AssignFocus(gameObject);
        }

        private void AssignFocus(GameObject gameObject)
        {
            var focusProviders = gameObject.GetComponents<IFocusConsumer>();
            foreach (var focusProvider in focusProviders)
            {
                focusProvider.SetFocus(_activeElements, _transformReferences, gameObject);
            }
        }

        public GameObject LastFocusedObject => _focusStack.Count > 0 ? _focusStack.Peek() : mainGameObjectInstance;

        public void ReturnToPreviousObject()
        {
            if (_focusStack.Count == 0) return;
            GameObject previousObject = _focusStack.Pop();
            Setup(previousObject);
        }

        private void AssignCamera(GameObject gameObject, Camera camera)
        {
            var mainCameraProvider = gameObject.GetComponent<IMainCameraProvider>();
            if (mainCameraProvider != null)
            {
                mainCameraProvider.MainCamera = camera;
            }
        }

        private void AssignMoveInput(GameObject gameObject)
        {
            var inputConsumer = gameObject.GetComponent<IMoveInputConsumer>();
            if (inputConsumer != null)
            {
                inputConsumer.EnableMoveInput(moveAction);
            }
        }

        private void UnLink(GameObject currentObject)
        {
            if (currentObject.TryGetComponent(out IMoveInputConsumer moveInputConsumer))
            {
                moveInputConsumer.DisableMoveInput(moveAction);
            }

            var mainCameraProviders = currentObject.GetComponents<IMainCameraProvider>();
            foreach (var mainCameraProvider in mainCameraProviders) mainCameraProvider.MainCamera = null;

            var focusProviders = currentObject.GetComponents<IFocusConsumer>();
            foreach (var focusProvider in focusProviders) focusProvider.OnFocusLost(currentObject);
        }

        public void Forget()
        {
            foreach (var activeUiElement in _activeElements)
            {
                Addressables.ReleaseInstance(activeUiElement.Value);
            }

            _activeElements.Clear();
            _focusStack.Clear();
            mainGameObjectInstance = null;
        }
    }
}