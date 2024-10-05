using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.Stats.Runtime.Model;
using Soul.Combats.Runtime;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public interface IWeapon : IWeaponBase<AttackOrigin, OffensiveStats<float>, Bullet>
    {
    }
}