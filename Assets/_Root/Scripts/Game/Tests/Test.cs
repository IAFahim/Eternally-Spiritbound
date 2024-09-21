using System;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Tests
{
    [Serializable]
    public class Test : MonoBehaviour
    {
        [Guid] public string guid;
        public XpLevel xpLevel;
        public StringIntStorage stringCountStorage;

        private void OnEnable()
        {
            stringCountStorage.LoadData(guid);
            int added;
            stringCountStorage.TryAdd("a", 1, out added);
            Debug.Log($"Added: {added}");
            stringCountStorage.SaveData();
            stringCountStorage.TryAdd("b", 2, out added);
            Debug.Log($"Added: {added}");
        }

        private void XpTest()
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