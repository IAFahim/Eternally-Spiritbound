using System;

namespace _Root.Scripts.Game.Stats.Runtime
{
    [Serializable]
    public class ProgressionStats
    {
        public float experience;
        public float luck;
        
        public static ProgressionStats Calculate(ProgressionStats baseStats, ProgressionStats multiplicative, ProgressionStats additive)
        {
            return new ProgressionStats
            {
                experience = baseStats.experience * (1 + multiplicative.experience) + additive.experience,
                luck = baseStats.luck * (1 + multiplicative.luck) + additive.luck
            };
        }
    }
}