using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Guid
{
    public class GuidProvider : MonoBehaviour, IGuidProvider
    {
        [Guid] public string guid;
        public string Guid => guid;
    }
}