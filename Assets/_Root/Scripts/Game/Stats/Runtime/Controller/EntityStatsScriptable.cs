using System.Collections.Generic;
using _Root.Scripts.Game.Guid;
using Sirenix.OdinInspector;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Stats.Runtime.Controller
{
    [CreateAssetMenu(fileName = "EntityStats", menuName = "Scriptable/Stats/EntityStats")]
    public class EntityStatsScriptable : ScriptableObject
    {
        public List<Pair<TitleGuid, EntityStats>> stats;
        private readonly Dictionary<TitleGuid, EntityStats> _dictionary = new();

        private void OnEnable()
        {
            foreach (var (key, value) in stats) _dictionary.TryAdd(key, value);
        }

        public EntityStats GetStats(TitleGuid titleGuid) => _dictionary[titleGuid];

        [Button]
        public void PopulateList()
        {
#if UNITY_EDITOR
            stats.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:TitleGuid");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var titleGuid = UnityEditor.AssetDatabase.LoadAssetAtPath<TitleGuid>(path);
                if (stats.Exists(pair => pair.Key == titleGuid)) continue;
                stats.Add(new Pair<TitleGuid, EntityStats>(titleGuid, new EntityStats()));
            }
#endif
        }
    }
}