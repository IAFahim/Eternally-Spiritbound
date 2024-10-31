using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public struct CheckConfig
    {
        public LayerMask checkMask;
        public OverlapType overlapType;
        public Transform checkPoint;
        public Vector3 positionOffset;

        public Vector3 boxSize;
        [Min(0f)] public float sphereRadius;

        public CheckConfig(LayerMask checkMask, OverlapType overlapType, Transform checkPoint, Vector3 positionOffset,
            float sphereRadius, Vector3 boxSize)
        {
            this.checkMask = checkMask;
            this.checkPoint = checkPoint;
            this.overlapType = overlapType;
            this.boxSize = boxSize;
            this.sphereRadius = sphereRadius;
            this.positionOffset = positionOffset;
        }
    }
}