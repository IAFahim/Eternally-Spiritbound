using _Root.Scripts.Game.FocusProvider.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    public abstract class FocusControllerCinemachineScriptable : FocusControllerScriptable
    {
        public AssetReferenceGameObject cinemachineAsset;
        protected GameObject TargetGameObject;
        private CinemachineCamera _cinemachineCache;

        protected void SetupCinemachine(GameObject spawnedCinemachine)
        {
            _cinemachineCache = spawnedCinemachine.GetComponent<CinemachineCamera>();
            _cinemachineCache.Follow = TargetGameObject.transform;
        }

        public override void OnFocusLost(GameObject targetGameObject)
        {
            _cinemachineCache.gameObject.SetActive(false);
        }
    }
}