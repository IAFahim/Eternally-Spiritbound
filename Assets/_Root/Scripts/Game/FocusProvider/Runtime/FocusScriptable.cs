using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Inputs.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public class FocusScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainObject;

        public Camera mainCamera;

        [Header("Input Actions")] [SerializeField]
        private InputActionReference moveAction;

        private TransformReferences _transformReferences;
        private Action<GameObject> _spawnedGameObjectCallBack;
        private readonly Dictionary<AssetReferenceGameObject, GameObject> _activeElements = new();

        // Stack to store the focused GameObjects
        private readonly Stack<FocusInfo> _focusStack = new();

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
            Push(new FocusInfo(handle.Result, true, null));
            _spawnedGameObjectCallBack?.Invoke(handle.Result);
            _spawnedGameObjectCallBack = null;
        }

        public void Push(FocusInfo focusInfo)
        {
            if(_focusStack.Count>0) UnLink(_focusStack.Peek().GameObject);
            _focusStack.Push(focusInfo);
            Setup(focusInfo);
        }


        private void Setup(FocusInfo focusInfo)
        {
            if (focusInfo.IsMain) mainObject = focusInfo.GameObject;
            AssignCamera(focusInfo.GameObject, mainCamera);
            AssignMoveInput(focusInfo.GameObject);
            AssignFocus(focusInfo.GameObject);
        }

        private void AssignFocus(GameObject gameObject)
        {
            var focusProviders = gameObject.GetComponents<IFocusConsumer>();
            foreach (var focusProvider in focusProviders)
            {
                focusProvider.SetFocus(_activeElements, _transformReferences, gameObject);
            }
        }

        [Button]
        public bool TryPopAndActiveLast()
        {
            if (!Pop()) return false;
            Setup(_focusStack.Peek());
            return true;
        }

        private bool Pop()
        {
            if (_focusStack.Count <= 1) return false;
            var lastFocusInfo = _focusStack.Pop();
            lastFocusInfo.Pop?.Invoke(this);
            return true;
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
        }
    }
}