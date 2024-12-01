using UnityEngine;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Asset/AssetView", fileName = "AssetView")]
    public abstract class View : ScriptableObject
    { 
        public abstract void Return();
    }
}