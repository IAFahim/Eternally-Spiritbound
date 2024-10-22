using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public class FocusManagerComponent : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        [FormerlySerializedAs("mainStackScriptable")] [FormerlySerializedAs("mainProviderScriptable")] public FocusScriptable focusScriptable;
        public TransformReferences transformReferences;

        private void Awake()
        {
            focusScriptable.Initialize(mainCamera, transformReferences);
            if (mainGameObjectInstance == null)
                focusScriptable.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                focusScriptable.Push(mainGameObjectInstance, true);
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            focusScriptable.Forget();
        }
    }
}