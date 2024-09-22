using System;
using UnityEngine;

namespace Soul2.Interactions.Runtime
{
    public interface IFollow
    {
        public Vector3 FollowOffset { get; }
        public Vector3 FollowPosition { get; protected set; }
        
        public void FollowTarget(Transform target);
        
        
    }
}