using System;
using UnityEngine;

namespace Soul.OverlapSugar.Runtime
{
    [Serializable]
    public class PhysicsCheck
    {
        public CheckConfig config;

        public bool Perform()
        {
            Vector3 position = config.checkPoint.TransformPoint(config.positionOffset);

            return config.overlapType switch
            {
                OverlapType.Sphere => CheckSphere(position),
                OverlapType.Box => CheckBox(position),
                _ => throw new ArgumentOutOfRangeException(nameof(OverlapType))
            };
        }

        private bool CheckBox(Vector3 position)
        {
            const float half = 0.5f;
            return Physics.CheckBox(position, config.boxSize * half, config.checkPoint.rotation,
                config.checkMask.value);
        }

        private bool CheckSphere(Vector3 position)
        {
            return Physics.CheckSphere(position, config.sphereRadius, config.checkMask.value);
        }


        public void DrawGizmos(Color checkColor, Color hitColor)
        {
#if UNITY_EDITOR
            if (config.checkPoint == null) return;

            bool hasHit = Perform();
            Gizmos.matrix = config.checkPoint.localToWorldMatrix;
            Gizmos.color = hasHit ? hitColor : checkColor;

            Vector3 pos = config.positionOffset;
            switch (config.overlapType)
            {
                case OverlapType.Sphere:
                    Gizmos.DrawWireSphere(pos, config.sphereRadius);
                    break;
                case OverlapType.Box:
                    Gizmos.DrawWireCube(pos, config.boxSize);
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
            config.checkPoint = newCheckPoint;
        }

        public void SetCheckType(OverlapType type) => config.overlapType = type;
        public void SetSearchMask(LayerMask newMask) => config.checkMask = newMask;
        public void SetOffset(Vector3 offset) => config.positionOffset = offset;
        public void SetBoxSize(Vector3 size) => config.boxSize = size;

        public void SetSphereRadius(float radius)
        {
#if DEBUG
            if (radius < 0f)
                throw new ArgumentOutOfRangeException(nameof(radius));
#endif
            config.sphereRadius = radius;
        }


        // Implicit operators for convenience
        public static implicit operator bool(PhysicsCheck check) => check.Perform();
        public static implicit operator LayerMask(PhysicsCheck check) => check.config.checkMask;
        
        
    }
}