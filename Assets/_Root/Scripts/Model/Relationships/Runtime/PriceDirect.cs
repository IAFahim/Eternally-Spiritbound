using _Root.Scripts.Model.Assets.Runtime;
using Soul.Relationships.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Relationships.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Structure/PriceDirect")]
    public class PriceDirect : Direct<AssetBase, float>
    {
    }
}