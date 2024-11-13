using System.Collections.Generic;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Asset/DataBase", fileName = "AssetDataBase")]
    public class AssetScriptDataBase : ScriptableSettings<AssetScriptDataBase>
    {
        public List<AssetScript> assets;
        private readonly Dictionary<string, AssetScript> _dictionary = new();

        private void OnEnable()
        {
            foreach (var asset in assets) _dictionary.Add(asset.Guid, asset);
        }

        public bool TryGetValue(string guid, out AssetScript assetScript) =>
            _dictionary.TryGetValue(guid, out assetScript);

        public AssetScript this[string guid] => _dictionary[guid];


#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void CaptureGuid()
        {
            assets.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:AssetScript");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var item = UnityEditor.AssetDatabase.LoadAssetAtPath<AssetScript>(path);
                assets.Add(item);
            }
        }
#endif
    }
}