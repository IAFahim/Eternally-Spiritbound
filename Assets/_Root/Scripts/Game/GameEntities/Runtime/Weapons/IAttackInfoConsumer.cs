using _Root.Scripts.Game.Combats.Runtime.Attacks;
using _Root.Scripts.Game.GameEntities.Runtime.Attacks;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public interface IAttackInfoConsumer
    {
        public AttackOrigin AttackOrigin { get; }
    }
}