using System;
using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.GameEntities.Runtime.Damages
{
    [Serializable]
    [CreateAssetMenu(fileName = "Damage Influence", menuName = "Scriptable/Influence/Damage")]
    public class DamageInfluenceScriptable: InfluenceScriptable<DamageType, float>
    {
    }
}