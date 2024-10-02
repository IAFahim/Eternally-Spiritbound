using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class OverlapSettings
    {
        [Header("Common")] [SerializeField] private LayerMask searchMask;
        [SerializeField] private Transform overlapPoint;

        [Header("Overlap Area")] [SerializeField]
        private OverlapType overlapType;

        [SerializeField] private Vector3 boxSize = Vector3.one;
        [SerializeField, Min(0f)] private float sphereRadius = 0.5f;

        [Header("Offset")] [SerializeField] private Vector3 positionOffset;

        [SerializeField] private LayerMask obstaclesMask;


        private int _size;

        public LayerMask SearchMask => searchMask;

        public OverlapType OverlapType => overlapType;

        public Vector3 Offset => positionOffset;

        public Vector3 BoxSize => boxSize;

        public float SphereRadius => sphereRadius;


        public LayerMask ObstaclesMask => obstaclesMask;

        private Collider[] overlapResults;

        public bool Initialized { get; private set; }


        public void Init(int resultsCapacity = 32)
        {
#if DEBUG
            if (resultsCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(resultsCapacity));
#endif
            overlapResults = new Collider[resultsCapacity];
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

        public void SetOverlapType(OverlapType type)
        {
            overlapType = type;
        }

        public void SetSearchMask(LayerMask newMask)
        {
            searchMask = newMask;
        }

        public void SetObstaclesMask(LayerMask newMask)
        {
            obstaclesMask = newMask;
        }

        public void SetOffset(Vector3 offset)
        {
            positionOffset = offset;
        }

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

        public int PerformOverlap(out Collider[] results)
        {
            Vector3 position = overlapPoint.TransformPoint(Offset);

            return OverlapType switch
            {
                OverlapType.Box => OverlapBox(position, out results),
                OverlapType.Sphere => OverlapSphere(position, out results),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }

        public int OverlapBox(Vector3 position, out Collider[] results)
        {
            const float half = 0.5f;

            int size = Physics.OverlapBoxNonAlloc(position,
                BoxSize * half,
                overlapResults,
                overlapPoint.rotation,
                SearchMask.value);

            results = overlapResults;
            return size;
        }


        public int OverlapSphere(Vector3 position, out Collider[] results)
        {
            int size = Physics.OverlapSphereNonAlloc(position,
                SphereRadius,
                overlapResults,
                SearchMask.value);
            results = overlapResults;
            return size;
        }


        public bool TryDrawGizmos(Color color)
        {
            if (overlapPoint == null)
                return false;

            Gizmos.matrix = overlapPoint.localToWorldMatrix;
            Gizmos.color = color;

            switch (overlapType)
            {
                case OverlapType.Box:
                    Gizmos.DrawCube(positionOffset, boxSize);
                    break;
                case OverlapType.Sphere:
                    Gizmos.DrawSphere(positionOffset, sphereRadius);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(overlapType));
            }

            return true;
        }
    }
}