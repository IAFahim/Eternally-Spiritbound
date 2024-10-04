using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class OverlapSettings
    {
        [Header("Common")] public LayerMask searchMask;
        public Transform overlapPoint;

        [Header("Overlap Area")] public OverlapType overlapType;

        public Vector3 boxSize = Vector3.one;
        [Min(0f)] public float sphereRadius = 0.5f;

        [Header("Offset")] [SerializeField] private Vector3 positionOffset;

        [Tooltip("Don't set it in Inspector, it shows the number of collider found")]
        public int foundSize;

        [NonSerialized] public Collider[] Colliders;

        public bool Initialized { get; private set; }

        public void Init(int resultsCapacity)
        {
#if DEBUG
            if (resultsCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(resultsCapacity));
#endif
            foundSize = 0;
            Colliders = new Collider[resultsCapacity];
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

        public void PerformOverlap()
        {
            PerformOverlap(out _);
        }

        public bool Found() => foundSize > 0;

        public int PerformOverlap(out Collider[] results)
        {
            Vector3 position = overlapPoint.TransformPoint(positionOffset);

            return overlapType switch
            {
                OverlapType.Box => OverlapBox(position, out results),
                OverlapType.Sphere => OverlapSphere(position, out results),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }

        public int OverlapBox(Vector3 position, out Collider[] results)
        {
            const float half = 0.5f;

            foundSize = Physics.OverlapBoxNonAlloc(position,
                boxSize * half,
                Colliders,
                overlapPoint.rotation,
                searchMask.value);

            results = Colliders;
            return foundSize;
        }


        public int OverlapSphere(Vector3 position, out Collider[] results)
        {
            foundSize = Physics.OverlapSphereNonAlloc(position,
                sphereRadius,
                Colliders,
                searchMask.value);
            results = Colliders;
            return foundSize;
        }

        public void DrawGizmos(Color overlapColor, Color foundColor)
        {
#if UNITY_EDITOR
            if (overlapPoint == null) return;

            Gizmos.matrix = overlapPoint.localToWorldMatrix;
            Gizmos.color = foundSize > 0 ? foundColor : overlapColor;

            switch (overlapType)
            {
                case OverlapType.Box:
                    Gizmos.DrawWireCube(positionOffset, boxSize);
                    break;
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(positionOffset, sphereRadius);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(overlapType));
            }

            Gizmos.color = foundColor;
            for (int i = 0; i < foundSize; i++) Gizmos.DrawWireSphere(Colliders[i].transform.position, 0.5f);
#endif
        }

        public static implicit operator LayerMask(OverlapSettings settings) => settings.searchMask;
        public static implicit operator OverlapType(OverlapSettings settings) => settings.overlapType;
        public static implicit operator int(OverlapSettings settings) => settings.foundSize;
        public static implicit operator bool(OverlapSettings settings) => settings.foundSize > 0;
        public static implicit operator Collider[](OverlapSettings settings) => settings.Colliders;
    }
}