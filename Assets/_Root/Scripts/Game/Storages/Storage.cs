using System;
using Pancake.Common;
using Soul2.Containers.RunTime;
using Soul2.Storages.Runtime;

namespace _Root.Scripts.Game.Storages
{
    [Serializable]
    public class Storage<T> : StorageBase<T>
    {
        public override string DataKey => $"StoItem_{Guid}";
        public override Pair<T, int>[] LoadData() => Data.Load(DataKey, DefaultData);
        public override void SaveData(Pair<T, int>[] data) => Data.Save(DataKey, data);
    }
}