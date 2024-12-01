using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using _Root.Scripts.Model.Stats.Runtime;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupDetectHandlerBase<T> : PickupHandlerBase<PickupContainerBase<T>>
    {
        [SerializeField] FocusManagerScript focusManager;
        private readonly List<PickupContainerBase<T>> checkIfCanBeAddedControllers = new();
        private EntityStatsComponent _entityStatsComponent;
        private float pickupRange;

        public override void Initialization()
        {
            focusManager.OnMainChanged += OnMainChanged;
            OnMainChanged(focusManager.mainObject);
        }

        private void OnMainChanged(GameObject obj)
        {
            _entityStatsComponent = obj.GetComponent<EntityStatsComponent>();
            _entityStatsComponent.RegisterChange(OnEntityStatsChange, OnOldEntityStatsCleanUp);
        }

        private void OnOldEntityStatsCleanUp() => _entityStatsComponent.entityStats.pickupRadius.OnChange -= OnPickupRadiusChange;

        private void OnEntityStatsChange()
        {
            _entityStatsComponent.entityStats.pickupRadius.OnChange += OnPickupRadiusChange;
            OnPickupRadiusChange(0, _entityStatsComponent.entityStats.pickupRadius);
        }

        private void OnPickupRadiusChange(float old, float current)
        {
            pickupRange = current;
        }

        public override void Handle(PickupContainerBase<T> responsibility)
        {
            responsibility.transform.gameObject.SetActive(true);
            checkIfCanBeAddedControllers.Add(responsibility);
        }

        public override void Process()
        {
            var targetPosition = _entityStatsComponent.transform.position;
            for (var i = checkIfCanBeAddedControllers.Count - 1; i >= 0; i--)
            {
                var controller = checkIfCanBeAddedControllers[i];
                if (Vector3.Distance(controller.transform.position, targetPosition) < pickupRange)
                {
                    HandleNext(controller);
                    checkIfCanBeAddedControllers.RemoveAt(i);
                }
            }
        }


        public override void Dispose()
        {
            checkIfCanBeAddedControllers.Clear();
            focusManager.OnMainChanged -= OnMainChanged;
        }


#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.green;
        public override void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            foreach (var container in checkIfCanBeAddedControllers)
                Gizmos.DrawWireSphere(container.transform.position, _entityStatsComponent.entityStats.pickupRadius);
        }
#endif
    }
}