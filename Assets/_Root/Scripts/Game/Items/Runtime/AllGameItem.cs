using Alchemy.Inspector;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime
{
    public class AllGameItem : ScriptableObject
    {
        public UnityDictionary<string ,GameItem> items;
        
        public GameItem this[string key]
        {
            get => items[key];
            set => items[key] = value;
        }
        
        public string this[GameItem key] => key;
        
        [Button]
        public void CaptureGuid()
        {
            var dictionary = new UnityDictionary<string, GameItem>();
            foreach (var (key, value) in items)
            {
                dictionary[value] = value;
            }
            items = dictionary;
        }
    }
}