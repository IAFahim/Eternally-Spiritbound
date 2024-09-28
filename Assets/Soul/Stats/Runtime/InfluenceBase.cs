using System;
using System.Collections.Generic;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Stats.Runtime
{
    [Serializable]
    public class InfluenceBase<T, TV> : ScriptableObject
    {
        [SerializeField] protected TV defaultInfluence;
        [SerializeField] protected UnityDictionary<T, TV> influences;

        public virtual TV GetStrength(T type) => influences.GetValueOrDefault(type, defaultInfluence);
    }
}