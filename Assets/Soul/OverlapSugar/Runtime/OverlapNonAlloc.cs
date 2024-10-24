using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class OverlapNonAlloc
    {
        protected const float Half = 0.5f;
        public LayerMask searchMask;
        public Transform overlapPoint;

        public OverlapType overlapType;

        public float sphereRadius = 0.5f;
        public Vector3 boxSize = Vector3.one;

        [SerializeField] protected Vector3 positionOffset;

        [Tooltip("The max number of colliders that can be found")]
        public int maxCapacity = 4;

        [FormerlySerializedAs("currentSize")]
        [Tooltip("Don't set it in Inspector, it shows the number of collider found")]
        public int colliderFound;

        protected Collider[] FoundColliders;

        public bool Initialized { get; private set; }

        public void Initialize()
        {
#if DEBUG
            if (maxCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity));
#endif
            colliderFound = 0;
            FoundColliders = new Collider[maxCapacity];
            Initialized = true;
        }


        public void SetOverlapPoint(Transform newOverlapPoint)
        {
#if DEBUG
            if (newOverlapPoint == null)
                throw new ArgumentNullException(nameof(newOverlapPoint));
#endif
            overlapPoint = newOverlapPoint;
        }

        public void SetOverlapType(OverlapType type) => overlapType = type;

        public void SetSearchMask(LayerMask newMask) => searchMask = newMask;
        public void SetOffset(Vector3 offset) => positionOffset = offset;

        public void SetBoxSize(Vector3 size)
        {
            boxSize = size;
        }

        public void SetSphereRadius(float radius)
        {
#if DEBUG
            if (radius < 0f)
                throw new ArgumentOutOfRangeException(nameof(radius));
#endif
            sphereRadius = radius;
        }

        public bool Found() => colliderFound > 0;

        public virtual int Perform()
        {
            return Perform(overlapPoint.TransformPoint(positionOffset));
        }

        protected int Perform(Vector3 position)
        {
            return overlapType switch
            {
                OverlapType.Sphere => OverlapSphere(position),
                OverlapType.Box => OverlapBox(position),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }


        public int GetColliders(out Collider[] foundColliders)
        {
            foundColliders = FoundColliders;
            return colliderFound;
        }

        private int OverlapBox(Vector3 position)
        {
            colliderFound = Physics.OverlapBoxNonAlloc(position,
                boxSize * Half,
                FoundColliders,
                overlapPoint.rotation,
                searchMask.value);
            return colliderFound;
        }


        private int OverlapSphere(Vector3 position)
        {
            colliderFound = Physics.OverlapSphereNonAlloc(position,
                sphereRadius,
                FoundColliders,
                searchMask.value);
            return colliderFound;
        }

        public bool TryGetClosest(out Collider closestCollider, out float distance)
        {
            closestCollider = null;
            distance = float.MaxValue;
            if (colliderFound == 0) return false;

            for (int i = 0; i < colliderFound; i++)
            {
                float currentDistance = Vector3.Distance(FoundColliders[i].transform.position, overlapPoint.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    closestCollider = FoundColliders[i];
                }
            }

            return true;
        }

        public bool TryGetFurthest(out Collider furthestCollider, out float distance)
        {
            furthestCollider = null;
            distance = float.MinValue;
            if (colliderFound == 0) return false;

            for (int i = 0; i < colliderFound; i++)
            {
                float currentDistance = Vector3.Distance(FoundColliders[i].transform.position, overlapPoint.position);
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                    furthestCollider = FoundColliders[i];
                }
            }

            return true;
        }

        public virtual void DrawGizmos(Color overlapColor, Color foundColor)
        {
#if UNITY_EDITOR
            if (overlapPoint == null) return;

            Gizmos.matrix = overlapPoint.localToWorldMatrix;
            Gizmos.color = colliderFound > 0 ? foundColor : overlapColor;

            switch (overlapType)
            {
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(positionOffset, sphereRadius);
                    break;
                case OverlapType.Box:
                    Gizmos.DrawWireCube(positionOffset, boxSize);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(overlapType));
            }

            Gizmos.color = foundColor;
            for (int i = 0; i < colliderFound; i++) Gizmos.DrawWireSphere(FoundColliders[i].transform.position, 0.5f);
#endif
        }
    }
}