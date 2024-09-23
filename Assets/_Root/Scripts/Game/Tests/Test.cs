using System;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Levels;
using _Root.Scripts.Game.QuickPickup.Runtime;
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


        public QuickItemPickupManager quickItemPickupManager;

        public GameObject itemGameObject;
        public GameItem gameItem;

        private void OnEnable()
        {
            quickItemPickupManager.Enable(gameItem);
            Target();
        }

        [Button]
        public void Target()
        {
            gameItem.TryDrop(null, transform.position);
        }

        private void OnDisable()
        {
            quickItemPickupManager.Disable();
        }

        private void Update()
        {
            quickItemPickupManager.Process();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            quickItemPickupManager?.OnDrawGizmos();
        }
#endif


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