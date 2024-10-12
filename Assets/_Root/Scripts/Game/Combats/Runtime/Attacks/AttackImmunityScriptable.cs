using System;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Combats.Runtime.Attacks
{
    [Serializable]
    [CreateAssetMenu(fileName = "Attack Influences", menuName = "Scriptable/Influences/Attack")]
    public class AttackImmunityScriptable : InfluenceScriptable<AttackType, float>
    {
    }
}