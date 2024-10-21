using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.MainProviders.Runtime
{
    public class MainStackControllerComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        [FormerlySerializedAs("mainProviderScriptable")] public MainStackScriptable mainStackScriptable;
        public TransformReferences transformReferences;

        private void Awake()
        {
            mainStackScriptable.Initialize(mainCamera, transformReferences);
            if (mainGameObjectInstance == null)
                mainStackScriptable.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                mainStackScriptable.Push(mainGameObjectInstance, true);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            mainStackScriptable.Forget();
        }
    }
}