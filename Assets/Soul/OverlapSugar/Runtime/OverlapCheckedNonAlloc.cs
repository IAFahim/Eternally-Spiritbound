using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class OverlapCheckedNonAlloc : OverlapNonAlloc
    {
        public override int Perform()
        {
#if DEBUG
            if (!Initialized)
                throw new InvalidOperationException("Must call Init before performing overlap checks");
#endif
            Vector3 position = overlapPoint.TransformPoint(positionOffset);

            // First, perform the quick check
            bool quickCheck = overlapType switch
            {
                OverlapType.Sphere => Physics.CheckSphere(position, sphereRadius, searchMask.value),
                OverlapType.Box => Physics.CheckBox(position, boxSize * Half, overlapPoint.rotation, searchMask.value),
                _ => throw new ArgumentOutOfRangeException(nameof(overlapType))
            };

            if (!quickCheck) return colliderFound = 0;
            return Perform(position);
        }


        public override void DrawGizmos(Color checkColor, Color hitColor)
        {
#if UNITY_EDITOR
            if (overlapPoint == null) return;

            Gizmos.color = colliderFound > 0 ? hitColor : checkColor;
            // overlapPoint.localToWorldMatrix but ignore scale
            Gizmos.matrix = Matrix4x4.TRS(overlapPoint.position, overlapPoint.rotation, Vector3.one);
            switch (overlapType)
            {
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(positionOffset, sphereRadius);
                    break;
                case OverlapType.Box:
                    Gizmos.DrawWireCube(positionOffset, boxSize);
                    break;
            }

            if (colliderFound > 0 && FoundColliders != null)
            {
                Gizmos.color = hitColor;
                for (int i = 0; i < colliderFound; i++)
                {
                    if (FoundColliders[i] != null)
                        Gizmos.DrawWireSphere(FoundColliders[i].transform.position, 0.5f);
                }
            }
#endif
        }
    }
}