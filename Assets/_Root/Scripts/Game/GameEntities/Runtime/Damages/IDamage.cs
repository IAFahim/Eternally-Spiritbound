using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Combats.Runtime.Damages;
using _Root.Scripts.Game.GameEntities.Runtime.Attacks;
using Soul.Stats.Runtime;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IDamage : IDamageBase<AttackOrigin, DamageInfo>
    {
    }
}