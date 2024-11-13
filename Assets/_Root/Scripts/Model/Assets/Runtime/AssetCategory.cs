using System;
using System.Collections.Generic;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [Serializable]
    public class AssetCategory
    {
        public string name;
        public List<AssetScript> assets;
    }
}