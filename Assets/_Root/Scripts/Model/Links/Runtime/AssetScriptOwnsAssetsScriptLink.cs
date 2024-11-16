using System.Collections.Generic;
using _Root.Scripts.Model.Assets.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Links.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Link/AssetOwnsAssets", fileName = "AssetOwnsAssets Link")]
    public class AssetScriptOwnsAssetsScriptLink : AssetScriptLink<List<AssetScript>>
    {
    }
}