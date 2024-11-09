using System;
using UnityEngine;

namespace _Root.Scripts.Game.Placements.Runtime
{
    [Serializable]
    public abstract class PlacementStrategy : ScriptableObject
    {
        public abstract int Place(Transform transform, int limit);
    }
}