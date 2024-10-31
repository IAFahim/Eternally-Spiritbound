using System;
using Pancake;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargeterComponent : MonoBehaviour<TargetingStrategy>, ITargeter
    {
        public event Action<ITargetable> OnTargetFound;
        public event Action<ITargetable, bool> OnTargetLost;

        
        [SerializeField] private TargetingStrategy targetingStrategy;
        [SerializeField] private bool hasTarget;
        
        [ShowInInspector] private ITargetable _currentTarget;

        public Transform Transform => transform;
        public TargetingStrategy TargetingStrategy => targetingStrategy;
        public ITargetable CurrentTarget => _currentTarget;
        public bool HasTarget => hasTarget;

        protected override void Init(TargetingStrategy argument) => targetingStrategy = argument;

        private void OnEnable()
        {
            hasTarget = false;
            if (targetingStrategy.TryGetTarget(out var targetable)) SetTarget(targetable);
            else targetingStrategy.OnFoundEvent += SetTarget;
            targetingStrategy.OnLostEvent += RemoveTarget;
        }

        protected virtual void SetTarget(ITargetable targetable)
        {
            if (targetable == null)
            {
                hasTarget = false;
                return;
            }

            _currentTarget = targetable;
            targetable.AddTargeter(this);
            OnTargetFound?.Invoke(targetable);
            hasTarget = true;
        }

        public virtual void RemoveTarget(ITargetable targetable, bool onDisable)
        {
            if (!onDisable) targetable.RemoveTargeter(this);
            OnTargetLost?.Invoke(targetable, onDisable);
            hasTarget = false;
        }

        private void OnDisable()
        {
            if (_currentTarget != null) _currentTarget.RemoveTargeter(this);
            targetingStrategy.OnLostEvent -= RemoveTarget;
            targetingStrategy.OnFoundEvent -= SetTarget;
        }
    }
}