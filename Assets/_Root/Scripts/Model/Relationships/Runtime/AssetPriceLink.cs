using System;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Relationships.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Relationships.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Graph/AssetPrice", fileName = "AssetPriceLink")]
    public class AssetPriceLink : Link<AssetScript, AssetPrice>
    {
        public override AssetScript GetSource(string source) => AssetScriptDataBase.Instance[source];
    }

    [Serializable]
    public struct AssetPrice
    {
        public AssetScript asset;
        public float price;
    }
}