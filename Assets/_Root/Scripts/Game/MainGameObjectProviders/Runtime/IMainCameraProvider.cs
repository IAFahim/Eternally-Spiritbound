using UnityEngine;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public interface IMainCameraProvider
    {
        Camera MainCamera { set; }
    }
}