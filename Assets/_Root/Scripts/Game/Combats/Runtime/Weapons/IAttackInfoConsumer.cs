using _Root.Scripts.Game.Combats.Runtime.Attacks;

namespace _Root.Scripts.Game.Combats.Runtime.Weapons
{
    public interface IAttackInfoConsumer
    {
        public Attack Attack { get; }
    }
}