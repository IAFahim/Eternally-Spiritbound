using Pancake.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Soul.Interactions.Runtime
{
    [CreateAssetMenu(fileName = "Drop Strategy", menuName = "Scriptable/New Drop Strategy")]
    public class DropStrategyScriptable : ScriptableObject
    {
        [SerializeField] private float dropRange = 1f;
        [SerializeField] private bool singleDrop;
        [SerializeField] private LayerMask dropLayerMask;
        public float DropRange => dropRange;

        public void OnDrop(AssetReferenceGameObject asset, Vector3 position, int amount)
        {
            // pick a random position around the position to drop the item, if not possible, drop it at the position
            if (singleDrop) DropSingle(asset, position);
            else
            {
                for (var i = 0; i < amount; i++) DropSingle(asset, position);
            }
        }

        protected virtual void DropSingle(AssetReferenceGameObject asset, Vector3 position)
        {
            var randomPosition = Random.insideUnitSphere * dropRange + position;
            randomPosition.y = position.y;
            var dropPosition = Physics.Raycast(randomPosition, Vector3.down, out var hit, dropRange, dropLayerMask)
                ? hit.point
                : position;
            SharedAssetReferencePool.Request(asset, dropPosition, Quaternion.identity);
        }
    }
}