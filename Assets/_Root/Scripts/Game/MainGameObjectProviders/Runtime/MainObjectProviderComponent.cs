using UnityEngine;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        public MainObjectProviderScriptable mainObjectProviderScriptable;
        public TransformReferences transformReferences;

        private void Awake()
        {
            mainObjectProviderScriptable.Initialize(mainCamera, transformReferences);
            if (mainGameObjectInstance == null)
                mainObjectProviderScriptable.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                mainObjectProviderScriptable.ProvideTo(mainGameObjectInstance);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            mainObjectProviderScriptable.Forget();
        }
    }
}