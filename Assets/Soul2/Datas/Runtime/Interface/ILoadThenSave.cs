namespace Soul2.Datas.Runtime.Interface
{
    public interface ILoadThenSave: IDataKey
    {
        public void FirstLoad(string guid);
        public void Save();
    }
}