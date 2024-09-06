
using UnityEngine;

namespace Links.Runtime
{
    public interface IPositionsAndRotationsProvider
    {
        public Vector3[] Positions { get; }
        public Quaternion[] Rotations { get; }
    }
}