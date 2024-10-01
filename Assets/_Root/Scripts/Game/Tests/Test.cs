using System;
using _Root.Scripts.Game.Items.Runtime;
using _Root.Scripts.Game.Items.Runtime.Storage;
using _Root.Scripts.Game.Levels.Runtime;
using _Root.Scripts.Game.QuickPickup.Runtime;
using _Root.Scripts.Game.Stats.Runtime;
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
        public ItemStorage stringCountStorage;
        


        public QuickItemPickupManager quickItemPickupManager;

        public GameObject itemGameObject;

        public GameItem itemBase;

        private void OnEnable()
        {
            // quickItemPickupManager.Enable(itemBase, 5, LayerMask.GetMask("Default"));
            Target();
        }

        [Button]
        public void Target()
        {
            itemBase.TrySpawn(transform.position, 1);
        }

        private void OnDisable()
        {
            quickItemPickupManager.Dispose();
        }

        private void Update()
        {
            // quickItemPickupManager.Process();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // quickItemPickupManager?.OnDrawGizmos();
        }
#endif
        

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