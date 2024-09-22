using System;
using Pancake.Common;
using Soul2.Containers.RunTime;
using Soul2.Levels.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Levels
{
    [Serializable]
    public class XpLevel : XpLevelBase
    {
        [SerializeField] private string appendKey = "_xp";
        public override string StorageKey => $"{Guid}{appendKey}";
        public override void LoadData(string guid)
        {
            Guid = guid;
            SetData(Data.Load(StorageKey, new Pair<int, int>(1, 0)));
        }

        public override void SaveData((int, int) data) => Data.Save(StorageKey, new Pair<int, int>(currentLevel, Xp));
        public override void SaveData(int data) => SaveData((data, Xp));
        public override void ClearStorage() => Data.DeleteKey(StorageKey);
    }
}