using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    public class GameItemStorageScriptable : ScriptableObject
    {
        public UnityDictionary<string, GameItemStorage> allStorages;
        public GameItemStorage GetStorage(string guid) => allStorages[guid];
    }
}