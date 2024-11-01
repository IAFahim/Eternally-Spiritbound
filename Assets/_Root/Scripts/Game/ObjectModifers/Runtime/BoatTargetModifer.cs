using _Root.Scripts.Game.Ai.Runtime.Movements;
using _Root.Scripts.Game.Ai.Runtime.Targets;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.ObjectModifers.Runtime
{
    [CreateAssetMenu(fileName = "Boat Target Modifier", menuName = "Scriptable/Object Modifer/Boat Targeter")]
    public class BoatTargetModifer : TargetModifier
    {
        [SerializeField] private BoatContextConfig boatContextConfig;

        public override void Modify(GameObject gameObject)
        {
            var targeterComponent = gameObject.AddComponent<TargeterComponent, TargetingStrategy>(targetingStrategy);
            gameObject.AddComponent<BoatContextSteering,
                BoatContextConfig, ITargeter>(boatContextConfig, targeterComponent);
        }

        public override void UnModify(GameObject gameObject)
        {
            Destroy(gameObject.GetComponent<TargeterComponent>());
            // Destroy(gameObject.GetComponent<BoatContextSteering>());
        }
    }
}