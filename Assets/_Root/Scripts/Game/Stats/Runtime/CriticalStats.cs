using System;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class CriticalStats<T>
    {
        public T chance;
        public T damage;
    }
}