using System;
using System.Collections.Generic;
using Soul2.QuickPickup.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.QuickPickup.Runtime.Handlers
{
    [Serializable]
    public class PickupHomingHandler<T> : PickupHandler<T>
    {
        [SerializeField] private float closeDistance = 0.5f;
        [SerializeField] private float attractSpeed = 5f;
        private readonly List<PickupContainer<T>> controllers = new();

        public override void Handle(PickupContainer<T> responsibility)
        {
            controllers.Add(responsibility);
        }

        public override void Process()
        {
            for (var index = controllers.Count - 1; index >= 0; index--)
            {
                var controller = controllers[index];
                var controllerPosition = controller.transform.position;
                var targetPosition = controller.otherTransform.position;
                var direction = (targetPosition - controllerPosition).normalized;
                controller.transform.position += direction * (attractSpeed * Time.deltaTime);

                if (Vector3.Distance(controllerPosition, targetPosition) < closeDistance)
                {
                    controllers.RemoveAt(index);
                    HandleNext(controller);
                }
            }
        }

        public override void Clear()
        {
            controllers.Clear();
        }

#if UNITY_EDITOR
        [SerializeField] protected Color gizmoColor = Color.blue;

        public override void OnDrawGizmos()
        {
            if (controllers == null) return;
            foreach (var controller in controllers)
            {
                Debug.DrawLine(controller.transform.position, controller.otherTransform.position, gizmoColor);
            }
        }
#endif
    }
}