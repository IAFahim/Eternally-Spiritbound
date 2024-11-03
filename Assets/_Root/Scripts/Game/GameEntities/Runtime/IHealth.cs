using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Modifiers.Runtime;

namespace _Root.Scripts.Game.GameEntities.Runtime
{
    public interface IHealth
    {
        public EnableLimitStat<Modifier> Value { get; }
    }
}