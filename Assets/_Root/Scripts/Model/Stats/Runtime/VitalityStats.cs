using System;
using UnityEngine;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public struct VitalityStats
    {
        private const float Half = .5f;
        public LimitStat health;
        public Vector3 size;


        public Vector3 Center(Transform transform) => Center(transform.position);
        public Vector3 Center(Vector3 position) => position + new Vector3(0, size.y * Half, 0);

        public Vector3 Bottom(Transform transform) => Bottom(transform.position);
        public Vector3 Bottom(Vector3 position) => position;

        public Vector3 Top(Transform transform) => Top(transform.position);
        public Vector3 Top(Vector3 position) => position + new Vector3(0, size.y, 0);
    }
}