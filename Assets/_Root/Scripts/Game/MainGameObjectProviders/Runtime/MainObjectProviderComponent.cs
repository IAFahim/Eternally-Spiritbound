using Unity.Cinemachine;
using UnityEngine;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public class MainObjectProviderComponent: MonoBehaviour
    {
        public CinemachineCamera virtualCamera;
        public MainObjectProviderScriptable mainObjectProviderScriptable;
        
        private void Start()
        {
            mainObjectProviderScriptable.SpawnMainGameObject(virtualCamera);
        }

        private void OnDisable()
        {
            mainObjectProviderScriptable.OnDisable();
        }
    }
}