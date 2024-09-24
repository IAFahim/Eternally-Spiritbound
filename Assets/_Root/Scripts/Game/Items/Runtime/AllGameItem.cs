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
    }
}