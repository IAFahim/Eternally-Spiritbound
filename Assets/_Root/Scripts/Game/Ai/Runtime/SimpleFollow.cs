using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.Ai
{
    public class SimpleFollow : MonoBehaviour
    {
        public Transform target;
        public float moveSpeed = 5f;
        public float rotationSpeed = 5f;
        public float stopDistance = 2f;

        private Vector3 lastSeenPosition;
        private Queue<Vector3> positionHistory = new Queue<Vector3>();
        private const int historySize = 10; // Adjust as needed

        private void Start()
        {
            lastSeenPosition = transform.position;
        }

        private void Update()
        {
            // 1. Update Target Position and History
            if (target != null)
            {
                if (positionHistory.Count >= historySize)
                {
                    positionHistory.Dequeue();
                }

                positionHistory.Enqueue(target.position);
                lastSeenPosition = target.position;
            }

            // 2. Determine Target Position (from history or last seen)
            Vector3 targetPosition = positionHistory.Count > 0 ? positionHistory.Peek() : lastSeenPosition;

            // 3. Stop if close enough to the target
            if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
            {
                return;
            }

            // 4. Move and Rotate towards the Target
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (target != null)
            {
                //draw history
                Gizmos.color = Color.red;
                foreach (var position in positionHistory)
                {
                    Gizmos.DrawSphere(position, 0.5f);
                }
            }
        }

#endif
    }
}