using System.Threading;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Root.Scripts.Presentation.Selectors.Runtime
{
    public class SelectorComponent : MonoBehaviour
    {
        [SerializeField] private MainObjectProviderScriptable mainObjectProvider;
        [SerializeField] private Selector<MainObjectProviderScriptable> selector;
        [SerializeField] private Camera camera;
        [SerializeField] private EventSystem eventSystem;
        private CancellationTokenSource cts;

        public void Awake()
        {
            cts = new CancellationTokenSource();
            selector.Initialize(mainObjectProvider, camera, eventSystem, OnOverUI, cts);
        }

        private void OnOverUI()
        {
            // Do nothing
        }

        public void OnEnable() => selector.Subscribe();

        public void OnDisable() => selector.Unsubscribe();
    }
}