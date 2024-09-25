using Soul.Modifiers.Runtime;

namespace Soul.Stats.Runtime
{
    public interface IHealthBase
    {
        Modifier CurrentHealth { get; }
        Modifier MaxHealth { get; }
    }
}