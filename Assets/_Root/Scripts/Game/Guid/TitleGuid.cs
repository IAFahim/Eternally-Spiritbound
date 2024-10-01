using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Guid
{
    public class TitleGuid : ScriptableConstant<string>
    {
        [SerializeField, Guid] public string guid;
    }
}