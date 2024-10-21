using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.MainProviders.Runtime
{
    public class MainProviderScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainObject;

        public Camera mainCamera;
        [Header("Input Actions")] 
        [SerializeField] private InputActionReference moveAction;

        private GameObject _currentInstance;
        private TransformReferences _transformReferences;
        private Action<GameObject> _spawnedGameObjectCallBack;
        private readonly Dictionary<AssetReferenceGameObject, GameObject> _activeElements = new();

        // Stack to store the focused GameObjects
        private readonly Stack<(GameObject gameObject, bool isMain)> _focusStack = new();

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
            _currentInstance = handle.Result;
            ProvideTo(_currentInstance, true);
            _spawnedGameObjectCallBack?.Invoke(_currentInstance);
            _spawnedGameObjectCallBack = null;
        }

        public void ProvideTo(GameObject gameObject, bool isMain)
        {
            if (_currentInstance != null)
            {
                UnLink(_currentInstance);
                _focusStack.Push((_currentInstance, isMain));
            }

            Setup(gameObject, isMain);
        }

        private void Setup(GameObject gameObject, bool isMain)
        {
            if (isMain) mainObject = gameObject;
            _currentInstance = gameObject;
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

        public void ReturnToPreviousObject()
        {
            if (_focusStack.Count == 0) return;
            var (gameObject, isMain) = _focusStack.Pop();
            Setup(gameObject, isMain);
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
            _currentInstance = null;
        }
    }
}