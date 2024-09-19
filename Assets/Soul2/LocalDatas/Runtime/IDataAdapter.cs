namespace Soul2.LocalDatas.Runtime
{
    public interface IDataAdapter<T>: IDataKey
    {
        public T DefaultData { get; }
        public T LoadRaw();
        public void SaveRaw(T data);
    }
}