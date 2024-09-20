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
        public override void SaveData(int data)
        {
            Data.Save(DataKey, data);
        }

        public override int LoadData()
        {
            return Data.Load(DataKey, DefaultData);
        }

        public override (int first, int second) LoadData2()
        {
            Pair<int, int> defaultData = DefaultData2;
            return Data.Load(DataKey, defaultData);
        }

        public override void SaveData2((int first, int second) data)
        {
            Data.Save(DataKey, new Pair<int, int>(data.first, data.second));
        }
    }

    public class Test : MonoBehaviour
    {
        [Guid] public string guid;
        public XpLevel xpLevel;
        public Pair<int, int> pair;

        private void OnEnable()
        {
            pair = new Pair<int, int>();
            // xpLevel.FirstLoad(guid);
            // xpLevel.AddXp(100);
            // xpLevel.Save();
            // Debug.Log($"Current Level: {xpLevel.CurrentLevel}");
            // Debug.Log($"Current XP: {xpLevel.Xp}");
            // Debug.Log($"XP to Next Level: {xpLevel.XpToNextLevel}");
            // Debug.Log($"XP Progress: {xpLevel.XpProgress}");
        }
    }
}