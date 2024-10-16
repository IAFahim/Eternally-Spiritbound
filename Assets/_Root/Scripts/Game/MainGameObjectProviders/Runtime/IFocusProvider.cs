using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.MainGameObjectProviders.Runtime
{
    public interface IFocusProvider
    {
        public void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            Transform uISpawnPointTransform, GameObject targetGameObject, Action returnFocusCallBack);

        public void OnFocusLost(GameObject targetGameObject);
    }
}