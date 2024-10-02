using _Root.Scripts.Game.Combats.Runtime.Attacks;
using Soul.Combats.Runtime;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public interface IWeapon : IWeaponBase<AttackOrigin, AttackInfo, WeaponStrategyBase>
    {
    }
}