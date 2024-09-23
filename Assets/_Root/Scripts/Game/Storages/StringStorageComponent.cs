using Soul2.Storages.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Storages
{
    public class StringStorageComponent : MonoBehaviour, IStringStorageReference
    {
        public StringIntStorage storage;
        public IStorageBase<string, int> Storage => storage;
    }
}