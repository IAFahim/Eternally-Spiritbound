using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Assets.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "Scriptable/Vehicles/Vehicle")]
    public class Vehicle : AssetBase, IDropStrategy
    {
        public DropStrategyScriptable dropStrategy;

        public float DropRange => dropStrategy.DropRange;

        public void OnDrop(GameObject user, Vector3 position, int amount)
        {
            dropStrategy.OnDrop(assetReferenceGameObject, position, amount);
        }
    }
}