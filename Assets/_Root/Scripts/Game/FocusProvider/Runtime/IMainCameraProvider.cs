using UnityEngine;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public interface IMainCameraProvider
    {
        Camera MainCamera { set; }
    }
}