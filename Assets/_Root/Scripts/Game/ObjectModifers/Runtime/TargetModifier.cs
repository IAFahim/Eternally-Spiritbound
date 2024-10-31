using _Root.Scripts.Game.Ai.Runtime.Targets;
using Sisus.Init.Reflection;
using UnityEngine;

namespace _Root.Scripts.Game.ObjectModifers.Runtime
{
    [CreateAssetMenu(fileName = "Target Modifier", menuName = "Scriptable/Object Modifer/Targeter")]
    public class TargetModifier : GameObjectModifer
    {
        [SerializeField] protected TargetingStrategy targetingStrategy;

        public override void Modify(GameObject gameObject)
        {
            gameObject.AddComponent<TargeterComponent>(targetingStrategy);
        }

        public override void UnModify(GameObject gameObject)
        {
            Destroy(gameObject.GetComponent<TargeterComponent>());
        }
    }
}