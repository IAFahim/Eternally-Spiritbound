using System;
using _Root.Scripts.Game.Combats.Runtime.Attacks;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Damages
{
    [Serializable]
    [CreateAssetMenu(fileName = "DamageInfluence", menuName = "Scriptable/Influence/DamageInfluence")]
    public class DamageInfluence: InfluenceBase<DamageType, float>
    {
    }
}