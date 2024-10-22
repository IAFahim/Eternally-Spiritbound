using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.FocusProvider.Runtime
{
    public interface IFocusConsumer: IFocusAble
    {
        public void SetFocus(Dictionary<AssetReferenceGameObject, GameObject> activeElements,
            TransformReferences transformReferences, GameObject targetGameObject);
        

        public void OnFocusLost(GameObject targetGameObject);
    }
}