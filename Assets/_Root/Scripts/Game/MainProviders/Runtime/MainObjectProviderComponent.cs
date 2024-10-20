using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        [FormerlySerializedAs("mainObjectProviderScriptable")] public MainProviderScriptable mainProviderScriptable;
        public TransformReferences transformReferences;

        private void Awake()
        {
            mainProviderScriptable.Initialize(mainCamera, transformReferences);
            if (mainGameObjectInstance == null)
                mainProviderScriptable.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                mainProviderScriptable.ProvideTo(mainGameObjectInstance);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            mainProviderScriptable.Forget();
        }
    }
}