namespace _Root.Scripts.Game.Inputs.Runtime
{
    public interface IAcceleration: IAccelerateInputConsumer
    {
        public float Acceleration { get; set; }
    }
}