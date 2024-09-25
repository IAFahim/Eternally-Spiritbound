using System.Collections.Generic;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Stats.Runtime
{
    public class DamageTypeStrength<T, TV> : ScriptableObject
    {
        [SerializeField] protected TV onMissingValue;
        [SerializeField] protected UnityDictionary<T, TV> strengths;

        public TV GetStrength(T type) => strengths.GetValueOrDefault(type, onMissingValue);
    }
}