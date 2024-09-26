namespace Soul.Datas.Runtime.Interface
{
    public interface IStorageAdapter<T> : IStorageIdentifier, IDataAdapter<T>, IClearStorage
    {
        void LoadData(string guid);
        void SaveData(T data);
        void SaveData();
    }
}