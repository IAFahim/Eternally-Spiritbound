using _Root.Scripts.Game.Inputs.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderScriptable : ScriptableObject
    {
        public AssetReferenceGameObject mainGameObjectAssetReference;
        public GameObject mainGameObjectInstance;
        public LayerMask layerMask;

        [Header("Input Actions")] public InputActionReference moveAction;
        public InputActionReference accelerateAction;

        public GameObject lastFocusedGameObject;
        private IMoveInputConsumer lastMoveInputConsumer;
        private IAccelerateInputConsumer lastAccelerateInputConsumer;

        public void SpawnMainGameObject(CinemachineCamera virtualCamera = null)
        {
            Addressables.InstantiateAsync(mainGameObjectAssetReference).Completed += handle =>
            {
                mainGameObjectInstance = handle.Result;
                if (virtualCamera) virtualCamera.Follow = mainGameObjectInstance.transform;
                ProvideTo(mainGameObjectInstance);
            };
        }

        public void ProvideTo(GameObject gameObject)
        {
            SetupGameObject(gameObject);
            SetupMoveInput(gameObject);
            SetupAccelerateInput(gameObject);
        }

        private void SetupGameObject(GameObject gameObject)
        {
            if (lastFocusedGameObject != null) lastFocusedGameObject.layer &= ~(1 << layerMask);
            lastFocusedGameObject = gameObject;
            lastFocusedGameObject.layer &= 1 << layerMask;
        }

        private void SetupMoveInput(GameObject gameObject)
        {
            lastMoveInputConsumer?.DisableMoveInput(moveAction);
            var inputConsumer = gameObject.GetComponent<IMoveInputConsumer>();
            if (inputConsumer == null) return;
            lastMoveInputConsumer = inputConsumer;
            lastMoveInputConsumer.EnableMoveInput(moveAction);
        }

        private void SetupAccelerateInput(GameObject gameObject)
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