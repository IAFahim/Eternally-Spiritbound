using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using Soul.Interactions.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Boats.Runtime
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "Scriptable/Asset/Vehicles/Vehicle")]
    public class VehicleAsset : AssetScript, IDropStrategy
    {
        public DropStrategyScriptable dropStrategy;

        public float DropRange => dropStrategy.DropRange;

        public void OnDrop(GameObject user, Vector3 position, int amount)
        {
            dropStrategy.OnDrop(AssetReference, position, amount);
        }
    }
}