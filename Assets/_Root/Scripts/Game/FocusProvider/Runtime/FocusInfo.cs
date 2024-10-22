using System;
using UnityEngine;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public class FocusInfo
    {
        public GameObject GameObject;
        public bool IsMain;
        public Action<FocusScriptable> OnPop;
        
        public FocusInfo(GameObject gameObject, bool isMain, Action<FocusScriptable> pop)
        {
            GameObject = gameObject;
            IsMain = isMain;
            OnPop = pop;
        }
    }
}