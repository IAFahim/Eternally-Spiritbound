using UnityEngine;

namespace Soul2.Interactions.Runtime
{
    public interface IRepel
    {
        public float RepelStrength { get; }
        public float RepelDuration { get; }
        public Vector3 RepelDirection { get; }
        public void RepelFrom(Vector3 sourcePosition, Vector3 sourceDirection);
    }
}