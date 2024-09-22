using System;
using _Root.Scripts.Game.Levels;
using _Root.Scripts.Game.PickableItems.Interaction;
using _Root.Scripts.Game.Storages;
using Alchemy.Inspector;
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
        public PickupItemManager pickupItemManager;
        public GameObject item;

        private void OnEnable()
        {
            pickupItemManager.AddItem(item, item.transform.position, 1);
        }

        private void OnDisable()
        {
            pickupItemManager.Clear();
        }

        private void Update()
        {
            pickupItemManager.CheckAll();
        }

        private void OnDrawGizmos()
        {
            pickupItemManager.OnDrawGizmos();
        }

        [Button]
        private void TestStorage()
        {
            stringCountStorage.LoadData(guid);
            int added;
            stringCountStorage.TryAdd("a", 1, out added);
            Debug.Log($"Added: {added}");
            stringCountStorage.SaveData();
            stringCountStorage.TryAdd("b", 2, out added);
            Debug.Log($"Added: {added}");
        }

        [Button]
        public void TestRemove()
        {
            stringCountStorage.RemoveAll("c", out var removed);
            Debug.Log($"Removed: {removed}");
            stringCountStorage.SaveData();
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