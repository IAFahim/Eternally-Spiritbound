using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime
{
    public class AllGameItem : ScriptableObject
    {
        public List<GameItem> gameItems;
        private readonly Dictionary<string, GameItem> _dictionary= new();

        private void OnEnable()
        {
            foreach (var item in gameItems) _dictionary.Add(item.guid, item);
        }

        public GameItem this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
        
        public string this[GameItem key] => key;
        
        [Button]
        public void CaptureGuid()
        {
            #if UNITY_EDITOR
            gameItems.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:GameItem");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var item = UnityEditor.AssetDatabase.LoadAssetAtPath<GameItem>(path);
                gameItems.Add(item);
            }
            #endif
        }
        
        
    }
}