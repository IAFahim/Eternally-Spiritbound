using System;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Relationships.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Relationships.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Graph/AssetPrice", fileName = "AssetPriceGraph")]
    public class AssetPriceGraph : Graph<AssetScript, AssetPrice>
    {
        public override AssetScript GetSource(string source) => AssetDataBase.Instance[source];
    }

    [Serializable]
    public struct AssetPrice
    {
        public AssetScript asset;
        public float price;
    }
}