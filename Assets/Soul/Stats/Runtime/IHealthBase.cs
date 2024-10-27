using _Root.Scripts.Game.Stats.Runtime.Model;

namespace Soul.Stats.Runtime
{
    public interface IHealthBase
    {
        public EnableLimitStat<float> Health { get; }
    }
}