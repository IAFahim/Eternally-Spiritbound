using System;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Links.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Links.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/link/AssetPrice", fileName = "Asset Price Link")]
    public class AssetPriceLink : Link<AssetScript, AssetPrice>
    {
        public override AssetScript GetSource(string source) => AssetScriptDataBase.Instance[source];
    }
    

    [Serializable]
    public struct AssetPrice
    {
        public AssetScript asset;
        public int price;
    }
}