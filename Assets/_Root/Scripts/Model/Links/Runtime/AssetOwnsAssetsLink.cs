using System.Collections.Generic;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Links.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Links.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Link/AssetOwnsAssets" , fileName = "Asset Owns Assets Link")]
    public class AssetOwnsAssetsLink : Link<AssetScript, List<AssetScript>>
    {
        public override AssetScript GetSource(string source) => AssetScriptDataBase.Instance[source];
    }
}