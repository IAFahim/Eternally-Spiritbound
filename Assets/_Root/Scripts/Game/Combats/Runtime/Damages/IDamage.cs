using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Stats.Runtime;

namespace _Root.Scripts.Game.Combats.Runtime.Damages
{
    public interface IDamage : IDamageBase<OffensiveStats<float>, DamageInfo>
    {
    }
}