using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class OverlapNonAlloc
    {
        protected const float Half = 0.5f;
        public LayerMask searchMask;
        public Transform overlapPoint;

        public OverlapType overlapType;

        public Vector3 boxSize = Vector3.one;
        [Min(0f)] public float sphereRadius = 0.5f;

        [SerializeField] protected Vector3 positionOffset;

        [Tooltip("Don't set it in Inspector, it shows the number of collider found")]
        public int foundSize;

        [NonSerialized] public Collider[] Colliders;

        public bool Initialized { get; private set; }

        public void Initialize(int resultsCapacity)
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

        public void SetSearchMask(LayerMask newMask) => searchMask = newMask;
        public void SetOffset(Vector3 offset) => positionOffset = offset;
        public void SetBoxSize(Vector3 size) => boxSize = size;

        public void SetSphereRadius(float radius)
        {
#if DEBUG
            if (radius < 0f)
                throw new ArgumentOutOfRangeException(nameof(radius));
#endif
            sphereRadius = radius;
        }

        public bool Found() => foundSize > 0;

        public virtual int Perform(out Collider[] results)
        {
            return Perform(overlapPoint.TransformPoint(positionOffset), out results);
        }

        protected int Perform(Vector3 position, out Collider[] results)
        {
            return overlapType switch
            {
                OverlapType.Sphere => OverlapSphere(position, out results),
                OverlapType.Box => OverlapBox(position, out results),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }

        private int OverlapBox(Vector3 position, out Collider[] results)
        {
            foundSize = Physics.OverlapBoxNonAlloc(position,
                boxSize * Half,
                Colliders,
                overlapPoint.rotation,
                searchMask.value);

            results = Colliders;
            return foundSize;
        }


        private int OverlapSphere(Vector3 position, out Collider[] results)
        {
            foundSize = Physics.OverlapSphereNonAlloc(position,
                sphereRadius,
                Colliders,
                searchMask.value);
            results = Colliders;
            return foundSize;
        }

        public virtual void DrawGizmos(Color overlapColor, Color foundColor)
        {
#if UNITY_EDITOR
            if (overlapPoint == null) return;

            Gizmos.matrix = overlapPoint.localToWorldMatrix;
            Gizmos.color = foundSize > 0 ? foundColor : overlapColor;

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
            for (int i = 0; i < foundSize; i++) Gizmos.DrawWireSphere(Colliders[i].transform.position, 0.5f);
#endif
        }
    }
}