using System;

namespace _Root.Scripts.Game.Stats.Runtime.Model
{
    [Serializable]
    public class EntityStatsBase<T> : ICloneable
    {
        public VitalityStats<T> vitality;
        public DefensiveStats<T> defensive;
        public MovementStats<T> movement;
        public ProgressionStats<T> progression;
        public CriticalStats<T> critical;
        public AmmoStats<T> ammo;
        public OffensiveStats<T> offensive;
        public object Clone() => MemberwiseClone();
    }
}