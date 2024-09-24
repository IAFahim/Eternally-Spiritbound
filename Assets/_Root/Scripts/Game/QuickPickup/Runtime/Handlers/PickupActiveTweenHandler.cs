using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Soul.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupActiveTweenHandler<T> : PickupHandler<T>
    {
        [SerializeField] private float repelDistance = 2f;
        [SerializeField] private float repelDuration = 0.5f;
        [SerializeField] private Ease repelEase = Ease.OutQuad;
        private readonly List<KeyValuePair<MotionHandle, PickupContainer<T>>> activeControllers = new();

        public override void Handle(PickupContainer<T> responsibility)
        {
            activeControllers.Add(new KeyValuePair<MotionHandle, PickupContainer<T>>(SetTween(responsibility), responsibility));
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

        private MotionHandle SetTween(PickupContainer<T> pickupContainer)
        {
            var pickupPosition = pickupContainer.transform.position;
            var direction = pickupPosition - pickupContainer.otherTransform.position;
            var repelPosition = pickupPosition + direction.normalized * repelDistance;
            return LMotion.Create(pickupContainer.startPosition, repelPosition, repelDuration)
                .WithEase(repelEase)
                .BindToPosition(pickupContainer.transform);
        }

        public override void Clear()
        {
            foreach (var activeController in activeControllers)
            {
                if (activeController.Key.IsActive()) activeController.Key.Cancel();
            }

            activeControllers.Clear();
        }

#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.red;
        public override void OnDrawGizmos()
        {
            if (activeControllers == null) return;
            Gizmos.color = gizmoColor;
            foreach (var activeController in activeControllers)
            {
                Gizmos.DrawLine(activeController.Value.transform.position, activeController.Value.otherTransform.position);
            }
        }
#endif
    }
}