using System.Threading;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Root.Scripts.Presentation.Selectors.Runtime
{
    public class SelectorComponent : MonoBehaviour
    {
        [SerializeField] private Selector selector;
        [SerializeField] private Camera camera;
        [SerializeField] private EventSystem eventSystem;
        private CancellationTokenSource cts;

        public void Awake()
        {
            cts = new CancellationTokenSource();
            selector.Initialize(camera, eventSystem, OnOverUI, cts);
        }

        private void OnOverUI()
        {
            // Do nothing
        }

        public void OnEnable() => selector.Subscribe();

        public void OnDisable() => selector.Unsubscribe();
    }
}