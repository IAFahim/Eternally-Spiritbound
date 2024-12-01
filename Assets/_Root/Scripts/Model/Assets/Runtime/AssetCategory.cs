using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [Serializable]
    public class AssetCategory
    {
        public string title;
        public View view;
        public bool gameObjectView = true;
        public Sprite icon;
        public List<AssetScript> assets;
    }
}