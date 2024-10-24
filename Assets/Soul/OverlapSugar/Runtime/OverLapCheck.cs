﻿using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{

    [Serializable]
    public class OverLapCheck
    {
        public LayerMask checkMask;
        public Transform checkPoint;

        public OverlapType overlapType;
        public Vector3 boxSize = Vector3.one;
        [Min(0f)] public float sphereRadius = 0.5f;

        [SerializeField] private Vector3 positionOffset;

        public bool Perform()
        {
            Vector3 position = checkPoint.TransformPoint(positionOffset);

            return overlapType switch
            {
                OverlapType.Sphere => CheckSphere(position),
                OverlapType.Box => CheckBox(position),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }

        private bool CheckBox(Vector3 position)
        {
            const float half = 0.5f;
            return Physics.CheckBox(position, boxSize * half, checkPoint.rotation, checkMask.value);
        }

        private bool CheckSphere(Vector3 position)
        {
            return Physics.CheckSphere(position, sphereRadius, checkMask.value);
        }
        

        public void DrawGizmos(Color checkColor, Color hitColor)
        {
#if UNITY_EDITOR
            if (checkPoint == null) return;

            bool hasHit = Perform();
            Gizmos.matrix = checkPoint.localToWorldMatrix;
            Gizmos.color = hasHit ? hitColor : checkColor;

            Vector3 pos = positionOffset;
            switch (overlapType)
            {
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(pos, sphereRadius);
                    break;
                case OverlapType.Box:
                    Gizmos.DrawWireCube(pos, boxSize);
                    break;
            }
#endif
        }

        // Utility methods for runtime configuration
        public void SetCheckPoint(Transform newCheckPoint)
        {
#if DEBUG
            if (newCheckPoint == null)
                throw new ArgumentNullException(nameof(newCheckPoint));
#endif
            checkPoint = newCheckPoint;
        }

        public void SetCheckType(OverlapType type) => overlapType = type;
        public void SetSearchMask(LayerMask newMask) => checkMask = newMask;
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
        

        // Implicit operators for convenience
        public static implicit operator bool(OverLapCheck check) => check.Perform();
        public static implicit operator LayerMask(OverLapCheck check) => check.checkMask;
    }
}