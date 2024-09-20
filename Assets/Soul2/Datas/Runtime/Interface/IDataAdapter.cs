namespace Soul2.Datas.Runtime.Interface
{
    public interface IDataAdapter<T> : IDataKey
    {
        T DefaultData { get; }
        T LoadData();
        void SaveData(T data);
    }

    public interface IDataAdapter<TFirst, TSecond> : IDataKey
    {
        (TFirst first, TSecond second) DefaultData2 { get; }
        (TFirst first, TSecond second) LoadData2();
        void SaveData2((TFirst first, TSecond second) data);
    }
    
    public interface IAdapter<T> : IDataKey
    {
        T DefaultData { get; }
        T LoadData(T data);
        void SaveData(T data);
    }
}