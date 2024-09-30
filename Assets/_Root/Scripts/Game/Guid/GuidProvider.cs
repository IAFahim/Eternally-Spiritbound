using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Guid
{
    [SelectionBase]
    public class GuidProvider : MonoBehaviour, IGuidProvider
    {
        [Guid] public string guid;
        public string Guid => guid;
        
        public static implicit operator string(GuidProvider provider) => provider.guid;
    }
}