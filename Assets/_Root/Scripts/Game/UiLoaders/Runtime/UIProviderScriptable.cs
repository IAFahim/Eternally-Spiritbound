using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.UiLoaders.Runtime
{
    public abstract class UIProviderScriptable : ScriptableObject, IUIProvider
    {
        public abstract void EnableUI(
            HashSet<GameObject> activeUiElementHashSet,
            Transform uISpawnPointTransform,
            GameObject gameObject
        );

        public abstract GameObject[] DisableUI(GameObject gameObject);
    }
}