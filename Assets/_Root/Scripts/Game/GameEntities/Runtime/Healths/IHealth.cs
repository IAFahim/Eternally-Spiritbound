using _Root.Scripts.Model.Stats.Runtime;
using Soul.Modifiers.Runtime;

namespace _Root.Scripts.Game.GameEntities.Runtime.Healths
{
    public interface IHealth
    {
        public LimitStat<Modifier> HealthReference { get; }
    }
}