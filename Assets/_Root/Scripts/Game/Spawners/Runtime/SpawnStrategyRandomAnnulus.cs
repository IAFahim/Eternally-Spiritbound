using UnityEngine;
using UnityUtils;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Spawn/Strategy/RandomAnnulus", fileName = "RandomAnnulus SpawnStrategy")]
    public class SpawnStrategyRandomAnnulus : SpawnStrategy
    {
        public int minRadius = 100;
        public int maxRadius = 150;

        public override int Spawn(Transform transform, Vector3 origin, int limit)
        {
            var position = origin.RandomPointInAnnulus(minRadius, maxRadius);
            transform.position = position;
            return 1;
        }
    }
}