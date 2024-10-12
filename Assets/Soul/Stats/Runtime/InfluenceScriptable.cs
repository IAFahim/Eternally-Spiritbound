using System;
using System.Collections.Generic;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace Soul.Stats.Runtime
{
    [Serializable]
    public class InfluenceScriptable<T, TV> : ScriptableObject
    {
        [SerializeField] protected TV defaultInfluence;
        [SerializeField] protected List<Pair<T, TV>> influences;

        public virtual TV GetInfluence(T type)
        {
            var influence = influences.Find(pair => pair.Key.Equals(type));
            return influence == null ? defaultInfluence : influence.Value;
        }
    }
}