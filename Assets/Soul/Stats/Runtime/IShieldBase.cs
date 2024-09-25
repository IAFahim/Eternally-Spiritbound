using Soul.Modifiers.Runtime;

namespace Soul.Stats.Runtime
{
    public interface IShieldBase
    {
        Modifier CurrentShield { get; }
        Modifier MaxShield { get; }
    }
}