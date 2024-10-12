using Soul.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Effects.Runtime
{
    [CreateAssetMenu(fileName = "Effect Influences", menuName = "Scriptable/Influences/Effect")]
    public class EffectInfluencesScriptable : InfluenceScriptable<string, float>
    {
        public float influenceMultiplier = 1;
        public override float GetInfluence(string type) => base.GetInfluence(type) * influenceMultiplier;
    }
}