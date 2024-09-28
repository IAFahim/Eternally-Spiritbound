namespace _Root.Scripts.Game.Interactables
{
    public interface IPickupStrategy
    {
        public bool AutoPickup { get; }
        public float PickupRange { get; }
    }
}