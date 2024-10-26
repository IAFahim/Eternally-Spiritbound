using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class FocusManagerInitialization : MonoBehaviour
    {
        public GameObject mainGameObjectInstance;
        public Camera mainCamera;
        public FocusManager focusManager;
        [FormerlySerializedAs("transformReferences")] public FocusReferences focusReferences;

        private void Awake()
        {
            focusManager.Initialize(mainCamera, focusReferences);
            if (mainGameObjectInstance == null)
                focusManager.SpawnMainGameObject(
                    SpawnedGameObjectCallBack
                );
            else
                focusManager.PushFocus(mainGameObjectInstance.GetComponent<IFocusEntryPoint>());
        }

        private void SpawnedGameObjectCallBack(GameObject obj) => mainGameObjectInstance = obj;

        private void OnDisable()
        {
            focusManager.Clear();
        }
    }
}