namespace Soul.Interactables.Runtime
{
    public interface IInteract<in T>
    {
        public void OnInteractStart(T initiator);
        public void OnInteractEnd(T initiator);
    }
}