using System.Collections.Generic;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Relationships.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Relationships.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Graph/AssetOwnsAssets" , fileName = "AssetOwnsAssetsGraph")]
    public class AssetOwnsAssetsGraph : Graph<AssetScript, List<AssetScript>>
    {
        public override AssetScript GetSource(string source) => AssetDataBase.Instance[source];
    }
}