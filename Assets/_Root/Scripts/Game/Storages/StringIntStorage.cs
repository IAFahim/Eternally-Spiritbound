using System;
using Pancake.Common;
using Soul2.Serializers.Runtime;
using Soul2.Storages.Runtime;

namespace _Root.Scripts.Game.Storages
{
    [Serializable]
    public class StringIntStorage : IntStorage<string>
    {
        public string appendKey = "_string_int";
        public override string StorageKey => $"{Guid}{appendKey}";

        public override void LoadData(string guid)
        {
            Guid = guid;
            SetData(Data.Load(StorageKey, DefaultData));
        }

        public override void SaveData(Pair<string, int>[] data) => Data.Save(StorageKey, data);
        public override void ClearStorage() => Data.DeleteKey(StorageKey);
    }
}