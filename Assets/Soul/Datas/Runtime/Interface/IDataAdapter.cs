namespace Soul.Datas.Runtime.Interface
{
    public interface IDataAdapter<T>
    {
        void GetDefaultData(out T dataDefault);
        void GetData(out T dataCurrent);
        void SetData(T dataNew);
    }
}