using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [Serializable]
    public class AssetCategory
    {
        [FormerlySerializedAs("name")] public string title;
        public Sprite icon;
        public List<AssetScript> assets;
    }
}