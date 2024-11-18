using System;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class CriticalStats
    {
        public float chance;
        [FormerlySerializedAs("damage")] public float damageMultiplier;
    }
}