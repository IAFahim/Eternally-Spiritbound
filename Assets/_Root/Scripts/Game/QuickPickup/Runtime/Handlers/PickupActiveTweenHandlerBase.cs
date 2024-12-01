using System;
using System.Collections.Generic;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using LitMotion;
using LitMotion.Extensions;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupActiveTweenHandlerBase<T> : PickupHandlerBase<PickupContainerBase<T>>
    {
        [SerializeField] private FocusManagerScript focusManager;
        [SerializeField] private float repelDistance = 2f;
        [SerializeField] private float repelDuration = 0.5f;
        [SerializeField] private float upHeight = 1f;
        [SerializeField] private Ease repelEase = Ease.OutQuad;
        private readonly List<KeyValuePair<MotionHandle, PickupContainerBase<T>>> activeControllers = new();
        
        private Transform _targetTransform;

        public override void Initialization()
        {
            focusManager.OnMainChanged += OnMainChanged;
            OnMainChanged(focusManager.mainObject);
        }

        private void OnMainChanged(GameObject obj)
        {
            _targetTransform = obj.transform;
        }

        public override void Handle(PickupContainerBase<T> responsibility)
        {
            activeControllers.Add(
                new KeyValuePair<MotionHandle, PickupContainerBase<T>>(SetTween(responsibility), responsibility));
        }

        public override void Process()
        {
            for (var i = activeControllers.Count - 1; i >= 0; i--)
            {
                var pair = activeControllers[i];
                if (!pair.Key.IsActive())
                {
                    HandleNext(pair.Value);
                    activeControllers.RemoveAt(i);
                }
            }
        }

        private MotionHandle SetTween(PickupContainerBase<T> pickupContainer)
        {
            var pickupPosition = pickupContainer.transform.position + new Vector3(0, upHeight, 0);
            var direction = pickupPosition - _targetTransform.position;
            var repelPosition = pickupPosition + direction.normalized * repelDistance;
            return LMotion.Create(pickupContainer.startPosition, repelPosition, repelDuration)
                .WithEase(repelEase)
                .BindToPosition(pickupContainer.transform);
        }

        public override void Dispose()
        {
            foreach (var activeController in activeControllers)
            {
                if (activeController.Key.IsActive()) activeController.Key.Cancel();
            }

            activeControllers.Clear();
            focusManager.OnMainChanged -= OnMainChanged;
        }

#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.red;
        public override void OnDrawGizmos()
        {
            if (activeControllers == null) return;
            Gizmos.color = gizmoColor;
            foreach (var activeController in activeControllers)
            {
                Gizmos.DrawLine(activeController.Value.transform.position, focusManager.mainObject.transform.position);
            }
        }
#endif
    }
}