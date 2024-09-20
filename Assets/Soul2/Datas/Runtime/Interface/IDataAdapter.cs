namespace Soul2.Datas.Runtime.Interface
{
    public interface IDataAdapter<T> : IStorageIdentifier
    {
        T DefaultData { get; }
        T LoadData();
        void SaveData(T data);
    }

    public interface IDataAdapter<TFirst, TSecond> : IStorageIdentifier
    {
        (TFirst first, TSecond second) DefaultData2 { get; }
        (TFirst first, TSecond second) LoadData2();
        void SaveData2((TFirst first, TSecond second) data);
    }

    public interface IStorageAdapter<in T> : IStorageIdentifier
    {
        void LoadData(string guid);
        void SetData(T data);
        void SaveData(T data);
        void SaveData();
    }
}