using System;
using _Root.Scripts.Model.Assets.Runtime;

namespace _Root.Scripts.Presentation.FocusProcessors.Runtime
{
    internal struct AssetInfoDto : IComparable<AssetInfoDto>
    {
        public int Index;
        public bool Unlocked;
        public AssetScript AssetScript;

        public AssetInfoDto(int index, bool unlocked, AssetScript assetScript)
        {
            Index = index;
            Unlocked = unlocked;
            AssetScript = assetScript;
        }

        public override string ToString()
        {
            return
                $"{nameof(Index)}: {Index}, {nameof(Unlocked)}: {Unlocked}";
        }

        public int CompareTo(AssetInfoDto other)
        {
            if (Unlocked && !other.Unlocked) return -1;
            if (!Unlocked && other.Unlocked) return 1;
            return Index.CompareTo(other.Index);
        }
    }
}