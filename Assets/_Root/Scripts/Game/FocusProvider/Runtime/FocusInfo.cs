using System;
using UnityEngine;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public struct FocusInfo
    {
        public GameObject GameObject;
        public bool IsMain;
        public Action<FocusScriptable> Pop;
        
        public FocusInfo(GameObject gameObject, bool isMain, Action<FocusScriptable> pop)
        {
            GameObject = gameObject;
            IsMain = isMain;
            Pop = pop;
        }
    }
}