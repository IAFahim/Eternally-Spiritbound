using _Root.Scripts.Model.Parameters.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime
{
    public class OffensiveStatsParameterScript : ParameterScript<OffensiveStats>
    {
        public void Combine(int level, OffensiveStats otherOffensiveStats)
        {
            TryGetParameter(level, out var offensiveStats);
            
        }
    }
}