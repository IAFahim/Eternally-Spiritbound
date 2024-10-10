using System;
using Pancake.Pools;
using UnityEngine;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [Serializable]
    public abstract class SpawnStrategy : ScriptableObject
    {
        public abstract int Spawn(AddressableGameObjectPool pool, Vector3 origin, int limit);
    }
}