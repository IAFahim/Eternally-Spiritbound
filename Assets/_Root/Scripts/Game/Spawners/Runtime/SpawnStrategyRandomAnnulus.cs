using _Root.Scripts.Game.Interactables.Runtime;
using UnityEngine;
using UnityUtils;

namespace _Root.Scripts.Game.Spawners.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Spawn/Strategy/RandomAnnulus", fileName = "RandomAnnulus SpawnStrategy")]
    public class SpawnStrategyRandomAnnulus : SpawnStrategy
    {
        public FocusManagerScript focusManager;
        public int minRadius = 100;
        public int maxRadius = 150;

        public override int Spawn(Transform transform, int limit)
        {
            var position = focusManager.MainObjectPosition.RandomPointInAnnulus(minRadius, maxRadius);
            transform.position = position;
            return 1;
        }
    }
}