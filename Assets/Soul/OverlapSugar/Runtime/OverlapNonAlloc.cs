using System;
using System.Collections.Generic;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public struct OverlapNonAlloc
    {
        internal const float Half = 0.5f;
        public OverlapConfig config;
        public Transform transform;
        public Transform rootTransform;
        
        // Instance-based filtered colliders list
        [NonSerialized] private List<Collider> filteredColliders;

        public void Initialize(Transform pointTransform)
        {
            rootTransform = pointTransform.root;
            SetOverlapPoint(pointTransform);
            config.Initialize();
            
            // Initialize the filtered colliders list with the same capacity
            filteredColliders = new List<Collider>(config.maxCapacity);
        }

        public void SetOverlapPoint(Transform newOverlapPoint)
        {
#if DEBUG
            if (newOverlapPoint == null)
                throw new ArgumentNullException(nameof(newOverlapPoint));
#endif
            transform = newOverlapPoint;
        }

        public void SetOverlapType(OverlapType type) => config.overlapType = type;
        public void SetSearchMask(LayerMask newMask) => config.searchMask = newMask;
        public void SetOffset(Vector3 offset) => config.positionOffset = offset;

        public void SetBoxSize(Vector3 size)
        {
            config.boxSize = size;
        }

        public void SetSphereRadius(float radius)
        {
#if DEBUG
            if (radius < 0f)
                throw new ArgumentOutOfRangeException(nameof(radius));
#endif
            config.sphereRadius = radius;
        }

        public bool Found() => config.colliderCount > 0;

        public int Perform()
        {
            // Ensure filteredColliders is initialized
            if (filteredColliders == null)
            {
                filteredColliders = new List<Collider>(config.maxCapacity);
            }

            if (config.checkFirst && config.colliderCount == 0)
            {
                var position = transform.TransformPoint(config.positionOffset);
                var quickCheck = config.overlapType switch
                {
                    OverlapType.Sphere => Physics.CheckSphere(position, config.sphereRadius, config.searchMask.value),
                    OverlapType.Box => Physics.CheckBox(
                        position, config.boxSize * Half, transform.rotation, config.searchMask.value
                    ),
                    _ => throw new ArgumentOutOfRangeException(nameof(config.overlapType))
                };

                if (!quickCheck) return config.colliderCount = 0;
            }

            var rawCount = Perform(transform.TransformPoint(config.positionOffset));
            return FilterColliders(rawCount);
        }

        private int Perform(Vector3 position)
        {
            return config.overlapType switch
            {
                OverlapType.Sphere => OverlapSphere(position),
                OverlapType.Box => OverlapBox(position),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }

        private int FilterColliders(int rawCount)
        {
            if (rawCount == 0) return 0;

            filteredColliders.Clear();
            
            for (int i = 0; i < rawCount; i++)
            {
                var collider = config.foundColliders[i];
                
                // Skip null colliders
                if (collider == null) continue;
                
                // Skip colliders with the same root transform
                if (collider.transform.root == rootTransform) continue;
                
                filteredColliders.Add(collider);
            }

            // Update the foundColliders array with filtered results
            config.colliderCount = filteredColliders.Count;
            for (int i = 0; i < config.colliderCount; i++)
            {
                config.foundColliders[i] = filteredColliders[i];
            }
            
            // Clear the remaining slots
            for (int i = config.colliderCount; i < rawCount; i++)
            {
                config.foundColliders[i] = null;
            }

            return config.colliderCount;
        }

        public int GetColliders(out Collider[] foundColliders)
        {
            foundColliders = config.foundColliders;
            return config.colliderCount;
        }

        private int OverlapBox(Vector3 position)
        {
            return Physics.OverlapBoxNonAlloc(position,
                config.boxSize * Half,
                config.foundColliders,
                transform.rotation,
                config.searchMask.value);
        }

        private int OverlapSphere(Vector3 position)
        {
            return Physics.OverlapSphereNonAlloc(position,
                config.sphereRadius,
                config.foundColliders,
                config.searchMask.value);
        }
        
        public bool TryGetClosest(out Collider closestCollider, out float distance)
        {
            closestCollider = null;
            distance = float.MaxValue;
            if (config.colliderCount == 0) return false;

            for (int i = 0; i < config.colliderCount; i++)
            {
                var currentCollider = config.foundColliders[i];
                if (currentCollider == null) continue;
                
                var currentDistance = Vector3.Distance(currentCollider.transform.position, transform.position);
                if (currentDistance > distance) continue;
                
                distance = currentDistance;
                closestCollider = currentCollider;
            }

            return closestCollider != null;
        }

        public bool TryGetFurthest(out Collider furthestCollider, out float distance)
        {
            furthestCollider = null;
            distance = float.MinValue;
            if (config.colliderCount == 0) return false;

            for (int i = 0; i < config.colliderCount; i++)
            {
                var currentCollider = config.foundColliders[i];
                if (currentCollider == null) continue;
                
                var currentDistance = Vector3.Distance(currentCollider.transform.position, transform.position);
                if (currentDistance < distance) continue;
                
                distance = currentDistance;
                furthestCollider = currentCollider;
            }

            return furthestCollider != null;
        }

        public void DrawGizmos(Color overlapColor, Color foundColor)
        {
#if UNITY_EDITOR
            if (transform == null) return;

            transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            Gizmos.color = config.colliderCount > 0 ? foundColor : overlapColor;

            switch (config.overlapType)
            {
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(config.positionOffset, config.sphereRadius);
                    break;
                case OverlapType.Box:
                    Gizmos.DrawWireCube(config.positionOffset, config.boxSize);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(config.overlapType));
            }

            Gizmos.color = foundColor;
            for (int i = 0; i < config.colliderCount; i++)
            {
                if (config.foundColliders[i] != null)
                    Gizmos.DrawWireSphere(config.foundColliders[i].transform.position, 0.5f);
            }
#endif
        }
    }
}