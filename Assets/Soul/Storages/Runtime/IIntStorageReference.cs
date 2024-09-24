namespace Soul.Storages.Runtime
{
    public interface IIntStorageReference<T>
    {
        public IStorageBase<T, int> Storage { get; }
    }
}