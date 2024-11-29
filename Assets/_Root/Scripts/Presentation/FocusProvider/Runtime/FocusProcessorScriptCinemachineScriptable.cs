using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Presentation.FocusProvider.Runtime
{
    public abstract class FocusProcessorScriptCinemachineScriptable : FocusProcessorScript
    {
        public AssetReferenceGameObject cinemachineAsset;
        protected GameObject TargetGameObject;
        private CinemachineCamera _cinemachineCache;

        protected void SetupCinemachine(GameObject spawnedCinemachine)
        {
            _cinemachineCache = spawnedCinemachine.GetComponent<CinemachineCamera>();
            _cinemachineCache.Follow = TargetGameObject.transform;
        }
    }
}