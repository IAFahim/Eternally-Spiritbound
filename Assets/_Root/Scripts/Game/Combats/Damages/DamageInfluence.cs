using System;
using _Root.Scripts.Game.Combats.Attacks;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Damages
{
    [Serializable]
    [CreateAssetMenu(fileName = "DamageInfluence", menuName = "Scriptable/Influence/DamageInfluence")]
    public class DamageInfluence: InfluenceBase<DamageType, float>
    {
    }
}