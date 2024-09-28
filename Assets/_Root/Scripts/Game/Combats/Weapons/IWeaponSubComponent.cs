using _Root.Scripts.Game.Combats.Attacks;

namespace _Root.Scripts.Game.Combats.Weapons
{
    public interface IWeaponSubComponent
    {
        public void Attack(Attack attack);
    }
}