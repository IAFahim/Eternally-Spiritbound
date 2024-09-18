namespace Soul2.LocalDatas.Runtime
{
    public interface ILocalData
    {
        public string LocalKey { get; }
        public void LocalLoad(string guid);
        public void LocalSave();
    }
}