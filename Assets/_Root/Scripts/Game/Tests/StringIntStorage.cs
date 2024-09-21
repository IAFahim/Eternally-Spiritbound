using System;
using Pancake.Common;
using Soul2.Containers.RunTime;
using Soul2.Storages.Runtime;

namespace _Root.Scripts.Game.Tests
{
    [Serializable]
    public class StringIntStorage : IntStorage<string>
    {
        public string appendKey = "_string_int";
        public string DataKey => $"{Guid}{appendKey}";

        public override void LoadData(string guid) => SetData(Data.Load(DataKey, startingElements));
        public override void SaveData(Pair<string, int>[] data) => Data.Save(DataKey, data);
    }
}