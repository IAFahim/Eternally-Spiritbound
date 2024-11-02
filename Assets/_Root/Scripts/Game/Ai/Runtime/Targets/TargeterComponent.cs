using System;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargeterComponent : MonoBehaviour<TargetStrategy>, ITargeter
    {
        public event Action<ITargetable> OnTargetFound;
        public event Action<ITargetable, bool> OnTargetLost;


        [FormerlySerializedAs("targetingStrategy")] [SerializeField] private TargetStrategy targetStrategy;
        [SerializeField] private bool hasTarget;

        [ShowInInspector] private ITargetable _currentTarget;

        public Transform Transform => transform;
        public TargetStrategy TargetStrategy => targetStrategy;
        public ITargetable CurrentTarget => _currentTarget;
        public bool HasTarget => hasTarget;

        protected override void Init(TargetStrategy argument) => targetStrategy = argument;

        private void OnEnable()
        {
            hasTarget = false;
            targetStrategy.Register(this, SetTarget, RemoveTarget);
        }

        protected virtual void SetTarget(ITargetable targetable)
        {
            if (_currentTarget != null) RemoveTarget(_currentTarget, false);
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
            targetStrategy.UnRegister(this, SetTarget, RemoveTarget);
        }
    }
}