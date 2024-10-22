using System.Threading;
using _Root.Scripts.Game.FocusProvider.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Selectors.Runtime
{
    public class SelectorComponent : MonoBehaviour
    {
        [FormerlySerializedAs("mainStack")] [SerializeField] private FocusScriptable focus;
        [SerializeField] private Selector<FocusScriptable> selector;
        [SerializeField] private Camera camera;
        [SerializeField] private EventSystem eventSystem;
        private CancellationTokenSource cts;

        public void Awake()
        {
            cts = new CancellationTokenSource();
            selector.Initialize(focus, camera, eventSystem, OnOverUI, cts);
        }

        private void OnOverUI()
        {
            // Do nothing
        }

        public void OnEnable() => selector.Subscribe();

        public void OnDisable() => selector.Unsubscribe();
    }
}