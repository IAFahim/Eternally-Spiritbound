using _Root.Scripts.Game.Weapons.Runtime.Attacks;

namespace _Root.Scripts.Game.Weapons.Runtime
{
    public interface IAttackInfoConsumer
    {
        public AttackOrigin AttackOrigin { get; }
    }
}