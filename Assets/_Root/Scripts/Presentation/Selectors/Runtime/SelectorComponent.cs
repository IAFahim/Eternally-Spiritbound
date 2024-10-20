﻿using System.Threading;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using Soul.Selectors.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Root.Scripts.Presentation.Selectors.Runtime
{
    public class SelectorComponent : MonoBehaviour
    {
        [FormerlySerializedAs("mainProvider")] [FormerlySerializedAs("mainObjectProvider")] [SerializeField] private MainStackScriptable mainStack;
        [SerializeField] private Selector<MainStackScriptable> selector;
        [SerializeField] private Camera camera;
        [SerializeField] private EventSystem eventSystem;
        private CancellationTokenSource cts;

        public void Awake()
        {
            cts = new CancellationTokenSource();
            selector.Initialize(mainStack, camera, eventSystem, OnOverUI, cts);
        }

        private void OnOverUI()
        {
            // Do nothing
        }

        public void OnEnable() => selector.Subscribe();

        public void OnDisable() => selector.Unsubscribe();
    }
}