using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Game.Interactables.Runtime.Focus;
using UnityEngine;
using UnityUtils;

namespace _Root.Scripts.Game.Placements.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/PlacementStrategy/RandomAnnulus", fileName = "RandomAnnulus placement strategy")]
    public class PlacementStrategyRandomAnnulus : PlacementStrategy
    {
        public FocusManagerScript focusManager;
        public int minRadius = 100;
        public int maxRadius = 150;

        public override int Place(Transform transform, int limit)
        {
            var position = focusManager.MainObjectPosition.RandomPointInAnnulus(minRadius, maxRadius);
            transform.position = position;
            return 1;
        }
    }
}