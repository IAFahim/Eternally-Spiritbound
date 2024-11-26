using System;
using _Root.Scripts.Model.Parameters.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
    [Serializable]
    public class OffensiveStatsParameters: Parameters<OffensiveStats>
    {
        public OffensiveStats Combine(int level, OffensiveStats otherOffensiveStats)
        {
            if (TryGetParameter(level, out var offensiveStats)) offensiveStats.Combine(otherOffensiveStats);
            return offensiveStats;
        }

        public bool TryCombine(int level, OffensiveStats otherOffensiveStats, out OffensiveStats offensiveStats)
        {
            var found = TryGetParameter(level, out offensiveStats);
            offensiveStats.Combine(otherOffensiveStats);
            return found;
        }
    }
}