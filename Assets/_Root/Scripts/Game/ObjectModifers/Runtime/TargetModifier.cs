using _Root.Scripts.Game.Ai.Runtime.Targets;
using Sisus.Init.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.ObjectModifers.Runtime
{
    [CreateAssetMenu(fileName = "Target Modifier", menuName = "Scriptable/Object Modifer/Targeter")]
    public class TargetModifier : GameObjectModifer
    {
        [FormerlySerializedAs("targetingStrategy")] [SerializeField] protected TargetStrategy targetStrategy;

        public override void Modify(GameObject gameObject)
        {
            gameObject.AddComponent<TargeterComponent>(targetStrategy);
        }

        public override void UnModify(GameObject gameObject)
        {
            Destroy(gameObject.GetComponent<TargeterComponent>());
        }
    }
}