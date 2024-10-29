using System;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargeterComponent : MonoBehaviour, ITargeter
    {
        [SerializeField] private TargetingStrategy targetingStrategy;
        public Transform Transform => transform;
        public TargetingStrategy TargetingStrategy => targetingStrategy;

        public event Action<ITargetable> OnTargetFound;
        public event Action<ITargetable, bool> OnTargetLost;


        protected ITargetable CurrentTarget;

        private void OnEnable()
        {
            CurrentTarget = null;
            if (targetingStrategy.TryGetTarget(out CurrentTarget)) SetTarget(CurrentTarget);
            else targetingStrategy.OnFoundEvent += SetTarget;
            targetingStrategy.OnLostEvent += RemoveTarget;
        }

        protected virtual void SetTarget(ITargetable targetable)
        {
            CurrentTarget = targetable;
            targetable.AddTargeter(this);
            OnTargetFound?.Invoke(targetable);
        }

        public virtual void RemoveTarget(ITargetable targetable, bool onDisable)
        {
            if (!onDisable) targetable.RemoveTargeter(this);
            OnTargetLost?.Invoke(targetable, onDisable);
            CurrentTarget = null;
        }

        private void OnDisable()
        {
            if (CurrentTarget != null) CurrentTarget.RemoveTargeter(this);
            targetingStrategy.OnLostEvent -= RemoveTarget;
            targetingStrategy.OnFoundEvent -= SetTarget;
        }
    }
}