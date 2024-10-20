using UnityEngine;

namespace _Root.Scripts.Game.MainProviders.Runtime
{
    public interface IMainCameraProvider
    {
        Camera MainCamera { set; }
    }
}