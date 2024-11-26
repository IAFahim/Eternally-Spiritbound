using System;
using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Weapons.Runtime.Damages
{
    [Serializable]
    [CreateAssetMenu(fileName = "Damage Influence", menuName = "Scriptable/Influence/Damage")]
    public class DamageInfluenceScriptable: InfluenceScriptable<DamageType, float>
    {
    }
}