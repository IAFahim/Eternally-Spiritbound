namespace Soul2.Datas.Runtime.Interface
{
    public interface ILoadThenSave: IStorageIdentifier
    {
        public void FirstLoad(string guid);
        public void Save();
    }
}