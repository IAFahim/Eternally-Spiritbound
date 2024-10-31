using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Inputs.Runtime;
using Pancake;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class FocusManagerScript : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainObject;
        
        public Vector3 MainObjectPosition => mainObject.transform.position;
        

        public Camera mainCamera;

        [Header("Input Actions")] [SerializeField]
        private InputActionReference moveAction;

        private FocusReferences _focusReferences;
        private Action<GameObject> _spawnedGameObjectCallBack;

        // Stack to store the focused GameObjects
        private readonly Stack<IFocusEntryPoint> _focusStack = new();

        public void Initialize(Camera camera, FocusReferences focusReferences)
        {
            mainCamera = camera;
            _focusReferences = focusReferences;
        }

        public void SpawnMainGameObject(Action<GameObject> gameObjectCallBack)
        {
            _spawnedGameObjectCallBack = gameObjectCallBack;
            Addressables.InstantiateAsync(mainGameObjectAssetReference).Completed += OnCompletedInstantiate;
        }

        public void PushFocus(IFocusEntryPoint focusEntryPoint)
        {
            if (_focusStack.Count > 0) UnLink(_focusStack.Peek().GameObject);
            _focusStack.Push(focusEntryPoint);
            Setup(focusEntryPoint);
        }

        [Button]
        public bool PopFocus()
        {
            if (!Pop()) return false;
            Setup(_focusStack.Peek());
            return true;
        }

        public IFocusEntryPoint PeekFocus() => _focusStack.Peek();

        private void OnCompletedInstantiate(AsyncOperationHandle<GameObject> handle)
        {
            PushFocus(handle.Result.GetComponent<IFocusEntryPoint>());
            _spawnedGameObjectCallBack?.Invoke(handle.Result);
            _spawnedGameObjectCallBack = null;
        }

        private void Setup(IFocusEntryPoint focusEntryPoint)
        {
            if (focusEntryPoint.IsMain) mainObject = focusEntryPoint.GameObject;
            AssignCamera(focusEntryPoint.GameObject, mainCamera);
            AssignMoveInput(focusEntryPoint.GameObject);
            AssignFocus(focusEntryPoint.GameObject);
        }

        private void AssignFocus(GameObject gameObject)
        {
            _focusReferences.currentGameObject = gameObject;
            var focusEntryPoint = gameObject.GetComponent<IFocusEntryPoint>();
            focusEntryPoint.PushFocus(_focusReferences);
        }


        private bool Pop()
        {
            if (_focusStack.Count <= 1) return false;
            _focusStack.Pop().IsFocused = false;
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
            currentObject.GetComponent<IFocusEntryPoint>().RemoveFocus(currentObject);
        }

        public void Clear()
        {
            _focusReferences.Clear();
            _focusStack.Clear();
            while (_focusStack.Count > 0) Pop();
        }
    }
}