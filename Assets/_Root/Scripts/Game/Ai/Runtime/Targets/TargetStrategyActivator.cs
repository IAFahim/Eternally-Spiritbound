using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargetStrategyActivator : MonoBehaviour
    {
        [SerializeField] private TargetingStrategy[] targetingStrategies;

        private void OnEnable()
        {
            foreach (var targetingStrategy in targetingStrategies) targetingStrategy.Activate();
        }

        private void OnDisable()
        {
            foreach (var targetingStrategy in targetingStrategies) targetingStrategy.Deactivate();
        }
    }
}