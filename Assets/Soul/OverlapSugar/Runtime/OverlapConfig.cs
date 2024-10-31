using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class OverlapConfig
    {
        public LayerMask searchMask;
        public OverlapType overlapType;
        public Vector3 positionOffset;
        public float sphereRadius;
        public Vector3 boxSize;

        [Tooltip("The max number of colliders that can be found")]
        public int maxCapacity;

        [Tooltip("Don't set it in Inspector, it shows the number of collider found")]
        public int colliderCount;

        public bool checkFirst;

        public Collider[] foundColliders;

        public OverlapConfig(LayerMask searchMask, OverlapType overlapType, Vector3 positionOffset, float sphereRadius,
            Vector3 boxSize, int maxCapacity, bool checkFirst)
        {
            this.searchMask = searchMask;
            this.overlapType = overlapType;
            this.positionOffset = positionOffset;
            this.sphereRadius = sphereRadius;
            this.boxSize = boxSize;
            this.maxCapacity = maxCapacity;
            this.checkFirst = checkFirst;
            Initialize();
        }

        public void Initialize()
        {
            colliderCount = 0;
            foundColliders = new Collider[maxCapacity];
        }
    }
}