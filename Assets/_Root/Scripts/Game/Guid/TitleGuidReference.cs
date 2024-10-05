using UnityEngine;

namespace _Root.Scripts.Game.Guid
{
    [SelectionBase]
    public class TitleGuidReference : MonoBehaviour, ITitleGuidReference
    {
        public TitleGuid titleGuid;
        public TitleGuid TitleGuid => titleGuid;
    }
}