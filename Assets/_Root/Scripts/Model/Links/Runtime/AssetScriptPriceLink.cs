using System;
using _Root.Scripts.Model.Assets.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Links.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Link/AssetPrice", fileName = "Asset Price Link")]
    public class AssetScriptPriceLink : AssetScriptLink<AssetPrice>
    {
    }
    

    [Serializable]
    public struct AssetPrice
    {
        public AssetScript asset;
        public int price;
    }
}