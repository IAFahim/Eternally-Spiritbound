using System;
using _Root.Scripts.Game.Combats.Runtime.Attacks;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Damages
{
    [Serializable]
    [CreateAssetMenu(fileName = "Damage Immunity", menuName = "Scriptable/Immunity/Damage")]
    public class DamageInfluence: InfluenceScriptable<DamageType, float>
    {
    }
}