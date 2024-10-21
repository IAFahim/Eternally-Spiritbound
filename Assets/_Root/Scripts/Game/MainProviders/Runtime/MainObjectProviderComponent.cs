using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.MainProviders.Runtime
{
    public class MainObjectProviderComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        public MainProviderScriptable mainProviderScriptable;
        public TransformReferences transformReferences;

        private void Awake()
        {
            mainProviderScriptable.Initialize(mainCamera, transformReferences);
            if (mainGameObjectInstance == null)
                mainProviderScriptable.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                mainProviderScriptable.ProvideTo(mainGameObjectInstance, true);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            mainProviderScriptable.Forget();
        }
    }
}