using System;
using Pancake;

namespace _Root.Scripts.Game.Persistent
{
    [EditorIcon("icon_default")]
    [Serializable]
    public class VibrationInitialization : BaseInitialization
    {
        public override void Init() { Vibration.Init(); }
    }
}