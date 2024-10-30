using System;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargeterComponent : MonoBehaviour<TargetingStrategy>, ITargeter
    {
        [SerializeField] private TargetingStrategy targetingStrategy;
        public Transform Transform => transform;

        public TargetingStrategy TargetingStrategy => targetingStrategy;
        public event Action<ITargetable> OnTargetFound;
        public event Action<ITargetable, bool> OnTargetLost;
        
        private ITargetable _currentTarget;
        
        protected override void Init(TargetingStrategy argument) => targetingStrategy = argument;

        private void OnEnable()
        {
            _currentTarget = null;
            if (targetingStrategy.TryGetTarget(out _currentTarget)) SetTarget(_currentTarget);
            else targetingStrategy.OnFoundEvent += SetTarget;
            targetingStrategy.OnLostEvent += RemoveTarget;
        }

        protected virtual void SetTarget(ITargetable targetable)
        {
            _currentTarget = targetable;
            targetable.AddTargeter(this);
            OnTargetFound?.Invoke(targetable);
        }

        public virtual void RemoveTarget(ITargetable targetable, bool onDisable)
        {
            if (!onDisable) targetable.RemoveTargeter(this);
            OnTargetLost?.Invoke(targetable, onDisable);
            _currentTarget = null;
        }

        private void OnDisable()
        {
            if (_currentTarget != null) _currentTarget.RemoveTargeter(this);
            targetingStrategy.OnLostEvent -= RemoveTarget;
            targetingStrategy.OnFoundEvent -= SetTarget;
        }

        
    }
}