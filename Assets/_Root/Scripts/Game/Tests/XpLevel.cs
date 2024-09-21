using System;
using Pancake.Common;
using Soul2.Containers.RunTime;
using Soul2.Levels.Runtime;

namespace _Root.Scripts.Game.Tests
{
    [Serializable]
    public class XpLevel : XpLevelBase
    {
        public string DataKey => $"{Guid}_xp";
        public override void LoadData(string guid) => SetData(Data.Load(DataKey, new Pair<int, int>(1, 0)));
        public override void SaveData((int, int) data) => Data.Save(DataKey, new Pair<int, int>(currentLevel, Xp));
        public override void SaveData(int data) => SaveData((data, Xp));
    }
}