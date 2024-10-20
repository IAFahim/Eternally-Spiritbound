using _Root.Scripts.Game.Interactables.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public class Vehicle : StringConstant, IDropStrategy
    {
        [Guid] public string guid;
        [SerializeField] private float dropRange;
        public float DropRange => dropRange;
        public AssetReferenceGameObject assetReferenceGameObject;
        public DropStrategyScriptable dropStrategy;

        public void OnDrop(GameObject user, Vector3 position, int amount)
        {
            dropStrategy.OnDrop(assetReferenceGameObject, position, amount);
        }
    }
}