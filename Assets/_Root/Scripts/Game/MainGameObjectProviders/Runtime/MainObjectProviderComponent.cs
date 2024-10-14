using Unity.Cinemachine;
using UnityEngine;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        public CinemachineCamera virtualCamera;
        public MainObjectProviderScriptable mainObjectProviderScriptable;
        public Transform uISpawnPoint;

        private void Awake()
        {
            if (mainGameObjectInstance == null)
                mainObjectProviderScriptable.SpawnMainGameObject(
                    mainCamera,
                    virtualCamera,
                    SpawnedGameObjectCallBack,
                    uISpawnPoint
                );
            else
                mainObjectProviderScriptable.ProvideTo(mainGameObjectInstance, mainCamera, virtualCamera, uISpawnPoint);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            mainObjectProviderScriptable.Forget();
        }
    }
}