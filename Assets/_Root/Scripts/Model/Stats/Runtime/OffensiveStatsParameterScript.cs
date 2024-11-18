using _Root.Scripts.Model.Parameters.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [CreateAssetMenu(fileName = "OffensiveStats", menuName = "Scriptable/Parameters/OffensiveStats")]
    public class OffensiveStatsParameterScript : ParameterScript<OffensiveStats>
    {
        public bool TryCombine(int level, OffensiveStats otherOffensiveStats, out OffensiveStats offensiveStats)
        {
            var found = TryGetParameter(level, out offensiveStats);
            offensiveStats.Combine(otherOffensiveStats);
            return found;
        }
    }
}