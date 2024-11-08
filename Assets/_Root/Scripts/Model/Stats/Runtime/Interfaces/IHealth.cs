using Soul.Modifiers.Runtime;

namespace _Root.Scripts.Model.Stats.Runtime.Interfaces
{
    public interface IHealth
    {
        public EnableLimitStat<Modifier> Health { get; }
    }
}