using System;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public abstract class SpawnStrategy : ScriptableObject
    {

        public abstract int Spawn(Transform transform, Vector3 origin, int limit);
    }
}