using _Root.Scripts.Game.FocusProvider.Runtime;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public class TargetComponent : MonoBehaviour, ITarget
    {
        [SerializeField] private Optional<GameObject> target;
        [SerializeField] private TargetingStrategy targetingStrategy;
        
        public Optional<GameObject> Target => target;

        private void Update()
        {
            if (targetingStrategy.DetectTarget(gameObject, out var newTarget))
            {
                target = newTarget;
            }
        }
    }
}