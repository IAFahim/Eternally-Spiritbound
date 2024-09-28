using _Root.Scripts.Game.Combats.Attacks;
using Soul.Combats.Runtime;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public interface IWeapon : IWeaponBase<AttackInfo, Attack, WeaponStrategyBase>
    {
    }
}