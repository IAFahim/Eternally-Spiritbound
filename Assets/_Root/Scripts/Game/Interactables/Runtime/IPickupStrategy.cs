namespace _Root.Scripts.Game.Interactables.Runtime
{
    public interface IPickupStrategy
    {
        public bool AutoPickup { get; }
        public float PickupRange { get; }
    }
}