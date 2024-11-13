using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Assets.Runtime
{
    public class AssetScriptComponent : MonoBehaviour
    {
        [FormerlySerializedAs("assetScript")] public AssetScript assetScriptReference;
    }
}