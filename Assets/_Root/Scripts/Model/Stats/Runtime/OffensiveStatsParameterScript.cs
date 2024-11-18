using _Root.Scripts.Model.Parameters.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
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