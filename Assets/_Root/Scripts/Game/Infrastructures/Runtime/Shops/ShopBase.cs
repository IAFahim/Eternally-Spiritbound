using System;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    [RequireComponent(typeof(InteractableEntryPointComponent))]
    public abstract class ShopBase : MonoBehaviour
    {
        [SerializeField] protected InteractableEntryPointComponent interactableEntryPointComponent;
        [SerializeField] private string[] tabs;
        public event Action OnShopUpdateEvent;

        protected virtual void OnEnable()
        {
            interactableEntryPointComponent.OnInteractionStartedEvent += OnEnter;
            interactableEntryPointComponent.OnInteractionEndedEvent += OnExit;
        }

        public abstract void OnEnter(IInteractorEntryPoint interactorEntryPoint);
        public abstract void OnUnlockedSelected(AssetScript assetScript);
        public abstract void OnDeSelected(AssetScript assetScript);
        public abstract void OnLockedItemSelected(AssetScript assetScript);
        public abstract void OnUnlocked(AssetScript assetScript);
        public abstract void OnExit(IInteractorEntryPoint interactorEntryPoint);

        protected virtual void OnDisable()
        {
            interactableEntryPointComponent.OnInteractionStartedEvent -= OnEnter;
            interactableEntryPointComponent.OnInteractionEndedEvent -= OnExit;
        }

        protected virtual void InvokeShopUpdateEvent() => OnShopUpdateEvent?.Invoke();

        protected virtual void OnValidate()
        {
            interactableEntryPointComponent ??= GetComponent<InteractableEntryPointComponent>();
        }
    }
}