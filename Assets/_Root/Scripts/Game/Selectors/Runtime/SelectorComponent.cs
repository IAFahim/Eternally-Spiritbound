using System.Threading;
using Cysharp.Threading.Tasks;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Root.Scripts.Game.Selectors.Runtime
{
    public class SelectorComponent : MonoBehaviour
    {
        [SerializeField] private Selector selector;
        [SerializeField] private Camera camera;
        [SerializeField] private EventSystem eventSystem;
        private CancellationToken cts;

        public void Awake()
        {
            cts = this.GetCancellationTokenOnDestroy();
            selector.Initialize( camera, eventSystem, OnOverUI, cts);
        }

        private void OnOverUI()
        {
            // Do nothing
        }

        public void OnEnable() => selector.Subscribe();

        public void OnDisable() => selector.Unsubscribe();
    }
}