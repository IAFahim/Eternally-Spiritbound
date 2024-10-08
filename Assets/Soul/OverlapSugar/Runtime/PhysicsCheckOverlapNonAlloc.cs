﻿using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class PhysicsCheckOverlapNonAlloc : OverlapNonAlloc
    {
        public override int Perform(out Collider[] results)
        {
#if DEBUG
            if (!Initialized)
                throw new InvalidOperationException("Must call Init before performing overlap checks");
#endif

            results = null;
            Vector3 position = overlapPoint.TransformPoint(positionOffset);

            // First, perform the quick check
            bool quickCheck = overlapType switch
            {
                OverlapType.Box => Physics.CheckBox(position, boxSize * Half, overlapPoint.rotation, searchMask.value),
                OverlapType.Sphere => Physics.CheckSphere(position, sphereRadius, searchMask.value),
                _ => throw new ArgumentOutOfRangeException(nameof(overlapType))
            };

            if (!quickCheck) return foundSize = 0;
            return Perform(position, out results);
        }


        public override void DrawGizmos(Color checkColor, Color hitColor)
        {
#if UNITY_EDITOR
            if (overlapPoint == null) return;

            Gizmos.matrix = overlapPoint.localToWorldMatrix;
            Gizmos.color = foundSize > 0 ? hitColor : checkColor;

            switch (overlapType)
            {
                case OverlapType.Box:
                    Gizmos.DrawWireCube(positionOffset, boxSize);
                    break;
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(positionOffset, sphereRadius);
                    break;
            }

            if (foundSize > 0 && Colliders != null)
            {
                Gizmos.color = hitColor;
                for (int i = 0; i < foundSize; i++)
                {
                    if (Colliders[i] != null)
                        Gizmos.DrawWireSphere(Colliders[i].transform.position, 0.5f);
                }
            }
#endif
        }
    }
}