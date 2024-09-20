using System;
using Pancake;
using Pancake.Common;
using Soul2.Containers.RunTime;
using Soul2.Levels.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Tests
{
    [Serializable]
    public class XpLevel : XpLevelBase
    {
        public string DataKey => $"{Guid}_xp";
        public override void LoadData(string guid) => SetData(Data.Load(DataKey, new Pair<int, int>(1, 0)));
        public override void SaveData((int, int) data) => Data.Save(DataKey, new Pair<int, int>(CurrentLevel, Xp));
        public override void SaveData(int data) => SaveData((data, Xp));
        public override void SaveData() => SaveData((CurrentLevel, Xp));
    }

    public class Test : MonoBehaviour
    {
        [Guid] public string guid;
        public XpLevel xpLevel;
        public Pair<int, int> pair;

        private void OnEnable()
        {
            xpLevel.Guid = guid;
            xpLevel.SaveData();
            xpLevel.LoadData(guid);
            xpLevel.AddXp(100);
            Debug.Log($"Current Level: {xpLevel.CurrentLevel}");
            Debug.Log($"Current XP: {xpLevel.Xp}");
            Debug.Log($"XP to Next Level: {xpLevel.XpToNextLevel}");
            Debug.Log($"XP Progress: {xpLevel.XpProgress}");
        }
    }
}