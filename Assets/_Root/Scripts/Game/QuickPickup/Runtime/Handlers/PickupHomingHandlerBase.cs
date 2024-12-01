using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using _Root.Scripts.Model.Assets.Runtime;
using Pancake.Pools;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupHomingHandlerBase<T> : PickupHandlerBase<PickupContainerBase<T>> where T : AssetScript
    {
        [SerializeField] private float closeDistance = 0.5f;
        [SerializeField] private float attractSpeed = 5f;
        [SerializeReference] private FocusManagerScript focusManagerScript;

        private readonly List<PickupContainerBase<T>> controllers = new();
        private Transform _targetTransform;
        private AssetScriptStorageComponent _assetScriptStorageComponent;

        public override void Initialization()
        {
            focusManagerScript.OnMainChanged += OnMainChanged;
            OnMainChanged(focusManagerScript.mainObject);
        }

        private void OnMainChanged(GameObject obj)
        {
            _targetTransform = obj.transform;
            _assetScriptStorageComponent = obj.GetComponent<AssetScriptStorageComponent>();
        }

        public override void Handle(PickupContainerBase<T> responsibility)
        {
            controllers.Add(responsibility);
        }

        public override void Process()
        {
            var targetPosition = _targetTransform.position;
            for (var index = controllers.Count - 1; index >= 0; index--)
            {
                var controller = controllers[index];
                var controllerPosition = controller.transform.position;
                var direction = (targetPosition - controllerPosition).normalized;
                controller.transform.position += direction * (attractSpeed * Time.deltaTime);

                if (Vector3.Distance(controllerPosition, targetPosition) < closeDistance)
                {
                    _assetScriptStorageComponent.AssetScriptStorage.TryAdd(
                        controller.element, controller.amount,
                        out _, out _
                    );
                    SharedAssetReferencePoolAsync.Return(controller.element, controller.transform.gameObject);
                    controllers.RemoveAt(index);
                }
            }
        }

        public override void Dispose()
        {
            controllers.Clear();
            focusManagerScript.OnMainChanged -= OnMainChanged;
        }

#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.blue;

        public override void OnDrawGizmos()
        {
            if (controllers == null) return;
            foreach (var controller in controllers)
            {
                Debug.DrawLine(controller.transform.position, _targetTransform.position, gizmoColor);
            }
        }
#endif
    }
}