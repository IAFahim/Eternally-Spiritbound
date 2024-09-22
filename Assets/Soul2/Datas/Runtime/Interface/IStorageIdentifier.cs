namespace Soul2.Datas.Runtime.Interface
{
    public interface IStorageIdentifier
    {
        public string Guid { get; set; }
        public string StorageKey { get; }
    }
}