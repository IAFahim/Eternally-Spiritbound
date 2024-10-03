namespace Soul.Datas.Runtime.Interface
{
    public interface IStorageAdapter<T> : IStorageInitializer, IStorageIdentifier, IDataAdapter<T>, IClearStorage
    {
        void LoadData(string guid);
        void SaveData(T data);
        void SaveData();
    }
}