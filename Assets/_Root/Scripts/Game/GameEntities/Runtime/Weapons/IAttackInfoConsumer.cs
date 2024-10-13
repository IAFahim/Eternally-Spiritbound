using _Root.Scripts.Game.GameEntities.Runtime.Attacks;

namespace _Root.Scripts.Game.GameEntities.Runtime.Weapons
{
    public interface IAttackInfoConsumer
    {
        public AttackOrigin AttackOrigin { get; }
    }
}