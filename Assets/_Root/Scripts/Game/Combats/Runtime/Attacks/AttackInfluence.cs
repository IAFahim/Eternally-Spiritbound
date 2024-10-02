using System;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    [Serializable]
    [CreateAssetMenu(fileName = "AttackInfluence", menuName = "Scriptable/Influence/AttackInfluence")]
    public class AttackInfluence : InfluenceBase<AttackType, float>
    {
        
    }
}