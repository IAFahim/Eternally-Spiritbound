using System;
using Pancake.Common;
using Soul2.Containers.RunTime;
using Soul2.Storages.Runtime;

namespace Game
{
    [Serializable]
    public class Storage<T> : StorageBase<T>
    {
        public override string DataKey => $"StoItem_{Guid}";
        public override Pair<T, int>[] LoadRaw() => Data.Load(DataKey, DefaultData);
        public override void SaveRaw(Pair<T, int>[] data) => Data.Save(DataKey, data);
    }
}