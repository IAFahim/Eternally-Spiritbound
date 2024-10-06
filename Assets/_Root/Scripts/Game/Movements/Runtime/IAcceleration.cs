using _Root.Scripts.Game.Inputs.Runtime;

namespace _Root.Scripts.Game.Movements.Runtime
{
    public interface IAcceleration: IAccelerateInputConsumer
    {
        public float Acceleration { get; set; }
    }
}