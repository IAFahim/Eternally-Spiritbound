using System;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    [CreateAssetMenu(fileName = "Drop Strategy", menuName = "Scriptable/New Drop Strategy")]
    public class DropStrategyScriptable : ScriptableObject
    {
        [SerializeField] private float dropRange = 1f;
        [SerializeField] private bool singleDrop;
        [SerializeField] private LayerMask dropLayerMask;
        public float DropRange => dropRange;

        public void OnDrop(AsyncAddressableGameObjectPool gameObjectPool, Vector3 position, int amount,
            Action<GameObject> spawnCallback)
        {
            // pick a random position around the position to drop the item, if not possible, drop it at the position
            if (singleDrop) DropSingle(gameObjectPool, position, spawnCallback);
            else
            {
                for (var i = 0; i < amount; i++) DropSingle(gameObjectPool, position, spawnCallback);
            }
        }

        private void DropSingle(AsyncAddressableGameObjectPool gameObjectPool, Vector3 position,
            Action<GameObject> spawnCallback)
        {
            var randomPosition = Random.insideUnitSphere * dropRange + position;
            randomPosition.y = position.y;
            if (Physics.Raycast(randomPosition, Vector3.down, out var hit, dropRange, dropLayerMask))
                Spawn(gameObjectPool, hit.point, spawnCallback).Forget();
            else Spawn(gameObjectPool, randomPosition, spawnCallback).Forget();
        }

        async UniTaskVoid Spawn(AsyncAddressableGameObjectPool pool, Vector3 position, Action<GameObject> spawnCallback)
        {
            var item = await pool.RentAsync(position, Quaternion.identity);
            item.transform.position = position;
            spawnCallback?.Invoke(item);
        }
    }
}