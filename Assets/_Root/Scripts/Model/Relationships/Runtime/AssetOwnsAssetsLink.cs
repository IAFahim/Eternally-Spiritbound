using System.Collections.Generic;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Relationships.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Relationships.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Link/AssetOwnsAssets" , fileName = "AssetOwnsAssetsLink")]
    public class AssetOwnsAssetsLink : Link<AssetScript, List<AssetScript>>
    {
        public override AssetScript GetSource(string source) => AssetScriptDataBase.Instance[source];
    }
}