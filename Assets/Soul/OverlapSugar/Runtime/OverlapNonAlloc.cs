using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public struct OverlapNonAlloc
    {
        internal const float Half = 0.5f;
        public OverlapConfig config;
        public Transform transform;

        public void Initialize()
        {
            config.Initialize();
        }

        public void Initialize(Transform pointTransform)
        {
            config.Initialize();
            transform = pointTransform;

#if DEBUG
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
#endif
        }

        public void SetOverlapPoint(Transform newOverlapPoint)
        {
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

            return Perform(transform.TransformPoint(config.positionOffset));
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


        public int GetColliders(out Collider[] foundColliders)
        {
            foundColliders = config.foundColliders;
            return config.colliderCount;
        }

        private int OverlapBox(Vector3 position)
        {
            config.colliderCount = Physics.OverlapBoxNonAlloc(position,
                config.boxSize * Half,
                config.foundColliders,
                transform.rotation,
                config.searchMask.value);
            return config.colliderCount;
        }


        private int OverlapSphere(Vector3 position)
        {
            config.colliderCount = Physics.OverlapSphereNonAlloc(position,
                config.sphereRadius,
                config.foundColliders,
                config.searchMask.value);
            return config.colliderCount;
        }

        public bool TryGetClosest(out Collider closestCollider, out float distance)
        {
            closestCollider = null;
            distance = float.MaxValue;
            if (config.colliderCount == 0) return false;

            for (int i = 0; i < config.colliderCount; i++)
            {
                var currentDistance = Vector3.Distance(config.foundColliders[i].transform.position, transform.position);
                if (currentDistance > distance) continue;
                distance = currentDistance;
                closestCollider = config.foundColliders[i];
            }

            return true;
        }

        public bool TryGetFurthest(out Collider furthestCollider, out float distance)
        {
            furthestCollider = null;
            distance = float.MinValue;
            if (config.colliderCount == 0) return false;

            for (int i = 0; i < config.colliderCount; i++)
            {
                float currentDistance =
                    Vector3.Distance(config.foundColliders[i].transform.position, transform.position);
                if (currentDistance < distance) continue;
                distance = currentDistance;
                furthestCollider = config.foundColliders[i];
            }

            return true;
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
                Gizmos.DrawWireSphere(config.foundColliders[i].transform.position, 0.5f);
#endif
        }
    }
}