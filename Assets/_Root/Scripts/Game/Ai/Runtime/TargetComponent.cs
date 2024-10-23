using _Root.Scripts.Game.FocusProvider.Runtime;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public class TargetComponent : MonoBehaviour, ITarget, IFocusConsumer
    {
        [SerializeField] private Optional<GameObject> target;
        [SerializeField] private TargetingStrategy targetingStrategy;
        public bool IsFocused { get; }

        public Optional<GameObject> Target => target;

        private void Update()
        {
            if (targetingStrategy.DetectTarget(gameObject, out var newTarget))
            {
                target = newTarget;
            }
        }


        public void SetFocus(FocusReferences focusReferences)
        {
            
        }

        public void OnFocusLost(GameObject targetGameObject)
        {
            throw new System.NotImplementedException();
        }
    }
}