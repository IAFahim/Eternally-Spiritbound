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
        public AssetCategory[] assetCategories;
        [SerializeField] protected InteractableEntryPointComponent interactableEntryPointComponent;
        [SerializeField] private bool debugEnabled;

        public event Action OnShopUpdateEvent;

        protected virtual void OnEnable()
        {
            interactableEntryPointComponent.OnInteractionStartedEvent += OnEnter;
            interactableEntryPointComponent.OnInteractionEndedEvent += OnExit;
        }


        public virtual void OnEnter(IInteractorEntryPoint interactorEntryPoint)
        {
            if (debugEnabled) return;
            Debug.Log($"[{name}] '{interactorEntryPoint.GameObject.name}' entered the shop", this);
        }

        public virtual void OnUnlockedSelected(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript)
        {
            if (debugEnabled) return;
            Debug.Log(
                $"[{name}] '{playerAssetScriptComponent.assetScriptReference.Value}' selected unlocked item '{assetScript.name}' from category '{category}'",
                this
            );
        }

        public virtual void OnDeSelected(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript)
        {
            Debug.Log(
                $"[{name}] '{playerAssetScriptComponent.assetScriptReference.Value}' deselected item '{assetScript.name}' from category '{category}'",
                this
            );
        }

        public virtual void OnLockedItemSelected(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript)
        {
            Debug.Log(
                $"[{name}] '{playerAssetScriptComponent.assetScriptReference.Value}' selected locked item '{assetScript.name}' from category '{category}'",
                this
            );
        }

        public virtual bool OnTryBuyButtonClick(AssetScriptComponent playerAssetScriptComponent, string category,
            AssetScript assetScript,
            out string message)
        {
            Debug.Log(
                $"[{name}] '{playerAssetScriptComponent.assetScriptReference.Value}' successfully purchased '{assetScript.name}' from category '{category}'",
                this
            );

            message = $"Successfully purchased {assetScript.name}!";
            return true;
        }

        public virtual void OnExit(IInteractorEntryPoint interactorEntryPoint)
        {
            Debug.Log(
                $"[{name}] '{interactorEntryPoint.GameObject.name}' exited the shop", this
            );
        }

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