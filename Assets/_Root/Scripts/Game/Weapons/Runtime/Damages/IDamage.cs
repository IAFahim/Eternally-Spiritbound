using _Root.Scripts.Game.GameEntities.Runtime.Damages;
using _Root.Scripts.Game.Weapons.Runtime.Attacks;
using Soul.Stats.Runtime;

namespace _Root.Scripts.Game.Weapons.Runtime.Damages
{
    public interface IDamage : IDamageBase<AttackOrigin, DamageInfo>
    {
    }
}