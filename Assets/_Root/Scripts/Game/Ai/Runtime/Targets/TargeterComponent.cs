using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargeterComponent : MonoBehaviour, ITargeter
    {
        [SerializeField] private TargetingStrategy targetingStrategy;
        public Transform Transform => transform;
        public TargetingStrategy TargetingStrategy => targetingStrategy;
        protected ITargetable CurrentTarget;

        private void OnEnable()
        {
            CurrentTarget = null;
            if (targetingStrategy.TryGetTarget(out CurrentTarget)) OnTargetFound(CurrentTarget);
            else targetingStrategy.OnFoundEvent += OnTargetFound;
            targetingStrategy.OnLostEvent += OnTargetLost;
        }

        protected virtual void OnTargetFound(ITargetable targetable)
        {
            CurrentTarget = targetable;
            targetable.AddTargeter(this);
        }

        protected virtual void OnTargetLost(ITargetable targetable)
        {
            targetable.RemoveTargeter(this);
            CurrentTarget = null;
        }

        private void OnDisable()
        {
            if (CurrentTarget != null) CurrentTarget.RemoveTargeter(this);
            targetingStrategy.OnLostEvent -= OnTargetLost;
            targetingStrategy.OnFoundEvent -= OnTargetFound;
        }
    }
}