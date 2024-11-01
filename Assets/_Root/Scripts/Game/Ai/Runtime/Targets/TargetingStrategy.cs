using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public event Action<ITargetable> OnFoundEvent;
        public event Action<ITargetable, bool> OnLostEvent;
        public abstract bool TryGetTarget([CanBeNull] ITargeter targeter, out ITargetable targetable);
        public abstract void StartTargetLookup();
        public abstract void TargetFound(ITargetable targetable);

        public abstract void TargetLost(ITargetable targetable, bool onDisable);

        public abstract void StopTargetLookup();

        protected void InvokeTargetFound(ITargetable targetable) => OnFoundEvent?.Invoke(targetable);

        protected void InvokeTargetLost(ITargetable targetable, bool onDisable) =>
            OnLostEvent?.Invoke(targetable, onDisable);
    }
}