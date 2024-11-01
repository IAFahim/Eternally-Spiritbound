using _Root.Scripts.Game.Ai.Runtime.Targets;
using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    [CreateAssetMenu(fileName = "BoatGame Activator", menuName = "Scriptable/Activators/BoatGame")]
    public class BoatGameActivateScript : ActivatorScript
    {
        [SerializeField] private TargetingStrategy targetingStrategy;

        public override void Activate(Transform activatorInvoker)
        {
            targetingStrategy.StartTargetLookup();
        }

        public override void Deactivate(Transform activatorInvoker)
        {
            targetingStrategy.StopTargetLookup();
        }
    }
}