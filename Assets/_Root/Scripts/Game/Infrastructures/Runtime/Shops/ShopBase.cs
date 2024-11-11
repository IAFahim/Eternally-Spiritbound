using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public abstract class ShopBase : MonoBehaviour
    {
        protected IInteractableEntryPoint InteractableEntryPoint;
        protected virtual void Awake() => InteractableEntryPoint = GetComponent<IInteractableEntryPoint>();

        protected virtual void OnEnable()
        {
            InteractableEntryPoint.OnInteractionStartedEvent += OnEnter;
            InteractableEntryPoint.OnInteractionEndedEvent += OnExit;
        }

        public abstract void OnEnter(IInteractorEntryPoint interactorEntryPoint);
        public abstract void OnEquipPlacement(Transform itemTransform);
        public abstract void OnUnEquipPlacement(Transform itemTransform);
        public abstract void OnLockedEquipPlacement(Transform itemTransform);
        public abstract void OnUnlockedEquipped();
        public abstract void OnExit(IInteractorEntryPoint interactorEntryPoint);

        protected virtual void OnDisable()
        {
            InteractableEntryPoint.OnInteractionStartedEvent -= OnEnter;
            InteractableEntryPoint.OnInteractionEndedEvent -= OnExit;
        }
    }
}