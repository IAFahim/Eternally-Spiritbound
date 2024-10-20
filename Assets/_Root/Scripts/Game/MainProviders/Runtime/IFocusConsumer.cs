using System.Collections.Generic;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.MainProviders.Runtime
{
    public interface IFocusConsumer
    {
        public bool IsFocused { get; }

        public void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences, GameObject targetGameObject);

        public void OnFocusLost(GameObject targetGameObject);
    }
}