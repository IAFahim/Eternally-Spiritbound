using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using Soul2.Containers.RunTime;
using Soul2.Interactions.Runtime;
using Soul2.Items.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.PickableItems.Interaction
{
    [Serializable]
    public class PickupItemManager : ScriptableObject, IRangeTrigger
    {
        public ItemBase item;
        [SerializeField] private List<PickController> inactiveControllers = new List<PickController>();

        [SerializeField] private List<Pair<MotionHandle, PickController>> activeControllers =
            new List<Pair<MotionHandle, PickController>>();

        [SerializeField] private List<PickController> homingControllers = new List<PickController>();

        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private float repelDistance = 2f;
        [SerializeField] private float repelDuration = 0.5f;
        [SerializeField] private Ease repelEase = Ease.OutQuad;
        [SerializeField] private float closeDistance = 0.5f;
        [SerializeField] private float attractSpeed = 5f;

        private int stage = 0;

        public float DetectionRadius => detectionRadius;

        public void AddItem(GameObject gameObject, Vector3 position, int amount)
        {
            inactiveControllers.Add(new PickController(gameObject.transform, amount));
        }

        public void CheckAll()
        {
            switch (stage)
            {
                case 0:
                    Detect();
                    break;
                case 1:
                    Repel();
                    break;
                case 2:
                    stage = -1;
                    break;
            }

            stage++;
            Attract();
        }

        private void Attract()
        {
            for (var index = homingControllers.Count - 1; index >= 0; index--)
            {
                var controller = homingControllers[index];
                var targetPosition = controller.collider.transform.position;
                var direction = (targetPosition - controller.transform.position).normalized;
                controller.transform.position += direction * (attractSpeed * Time.deltaTime);

                if (Vector3.Distance(controller.transform.position, targetPosition) < closeDistance)
                {
                    // Item picked up, handle accordingly (e.g., increase player's item count)
                    homingControllers.RemoveAt(index);
                    GameObject.Destroy(controller.transform.gameObject);
                }
            }
        }

        private void Repel()
        {
            for (var i = activeControllers.Count - 1; i >= 0; i--)
            {
                var pair = activeControllers[i];
                if (!pair.Key.IsActive())
                {
                    homingControllers.Add(pair.Value);
                    activeControllers.RemoveAt(i);
                }
            }
        }

        private void Detect()
        {
            for (var i = inactiveControllers.Count - 1; i >= 0; i--)
            {
                var pickController = inactiveControllers[i];
                if (pickController.TryActive(detectionRadius, layerMask, out var activePickController))
                {
                    activeControllers.Add(
                        new(RepelActivePickControllers(activePickController), activePickController)
                    );
                    inactiveControllers.RemoveAt(i);
                }
            }
        }

        public void RemoveAndDirectFollow(PickController pickController)
        {
            activeControllers.Add(new(RepelActivePickControllers(pickController), pickController));
        }

        public MotionHandle RepelActivePickControllers(PickController pickController)
        {
            var direction = pickController.startPosition - pickController.collider.transform.position;
            var repelPosition = pickController.startPosition + direction.normalized * repelDistance;
            return LMotion.Create(pickController.startPosition, repelPosition, repelDuration)
                .WithEase(repelEase)
                .BindToPosition(pickController.transform);
        }

        public void Clear()
        {
            foreach (var pickController in inactiveControllers) pickController.transform = pickController;
            inactiveControllers.Clear();

            foreach (var activeController in activeControllers)
            {
                if (activeController.Key.IsActive()) activeController.Key.Complete();
            }

            activeControllers.Clear();

            foreach (var pickController in homingControllers) pickController.transform = pickController;
            homingControllers.Clear();
        }

#if UNITY_EDITOR
        public Color gizmoColor = Color.green;
        public void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            foreach (var controller in inactiveControllers)
            {
                Gizmos.DrawWireSphere(controller.startPosition, detectionRadius);
            }
        }
#endif
    }
}