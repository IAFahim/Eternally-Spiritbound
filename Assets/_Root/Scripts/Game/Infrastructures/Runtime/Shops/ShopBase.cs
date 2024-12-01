using System;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Links.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    [RequireComponent(typeof(InteractableEntryPointComponent))]
    public abstract class ShopBase : MonoBehaviour
    {
        public string equippedItemGuid;
        [SerializeField] protected InteractableEntryPointComponent interactableEntryPointComponent;
        [SerializeField] private bool debugEnabled;
        
        public GameObject currentPreviewGameObject;
        
        public event Action OnShopUpdateEvent;

        protected virtual void OnEnable()
        {
            interactableEntryPointComponent.OnStartedEvent += OnEnter;
            interactableEntryPointComponent.OnEndedEvent += OnExit;
        }
        
        public abstract AssetCategory[] GetAssetCategories();

        public abstract void OnEnter(IInteractorEntryPoint interactorEntryPoint);

        public abstract void OnUnlockedSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript);

        public abstract void OnDeSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript);

        public abstract void OnLockedItemSelected(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript);

        public abstract bool OnTryBuyButtonClick(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            string category,
            AssetScript assetScript,
            out string message);

        public abstract bool HasEnough(AssetScriptReferenceComponent playerAssetScriptReferenceComponent,
            AssetScript item,
            out AssetPrice assetPrice);

        public abstract void OnExit(IInteractorEntryPoint interactorEntryPoint);

        protected virtual void OnDisable()
        {
            interactableEntryPointComponent.OnStartedEvent -= OnEnter;
            interactableEntryPointComponent.OnEndedEvent -= OnExit;
        }

        protected virtual void InvokeShopUpdateEvent() => OnShopUpdateEvent?.Invoke();

        protected virtual void OnValidate()
        {
            interactableEntryPointComponent ??= GetComponent<InteractableEntryPointComponent>();
        }
    }
}