namespace Soul2.LocalDatas.Runtime
{
    public interface ILoadThenSave: IDataKey
    {
        public void FirstLoad(string guid);
        public void Save();
    }
}