using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Storages.Runtime
{
    public class GameItemStorageScriptable : ScriptableObject
    {
        public UnityDictionary<string, GameItemStorage> allStorages;
        public GameItemStorage GetStorage(string guid) => allStorages[guid];
    }
}