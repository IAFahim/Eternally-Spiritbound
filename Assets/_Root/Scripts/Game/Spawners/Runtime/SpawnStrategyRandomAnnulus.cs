using Pancake.Pools;
using UnityEngine;
using UnityUtils;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/SpawnStrategy/RandomAnnulus")]
    public class SpawnStrategyRandomAnnulus : SpawnStrategy
    {
        public int minRadius = 100;
        public int maxRadius = 150;

        public override int Spawn(AddressableGameObjectPool pool, Vector3 origin, int limit)
        {
            var position = origin.RandomPointInAnnulus(minRadius, maxRadius);
            pool.Request(position, Quaternion.identity);
            return 1;
        }
    }
}