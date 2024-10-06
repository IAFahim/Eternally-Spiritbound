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

        private void Start()
        {
            if (mainGameObjectInstance == null)
            {
                mainObjectProviderScriptable.SpawnMainGameObject(mainCamera, SpawnedGameObjectCallBack, virtualCamera);
            }
            else mainObjectProviderScriptable.ProvideTo(mainGameObjectInstance);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            mainObjectProviderScriptable.OnDisable();
        }
    }
}