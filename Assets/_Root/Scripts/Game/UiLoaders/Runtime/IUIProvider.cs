using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.UiLoaders.Runtime
{
    public interface IUIProvider
    {
        public void EnableUI(HashSet<GameObject> activeUiElementHashSet, Transform uISpawnPointTransform, GameObject gameObject);
        public GameObject[] DisableUI(GameObject gameObject);
    }
}