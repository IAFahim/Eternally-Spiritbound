using System;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public event Action<ITargetable> OnFoundEvent;
        public event Action<ITargetable, bool> OnLostEvent;

        public abstract void Activate();
        public abstract bool TryGetTarget(out ITargetable targetable);

        public virtual void TargetFound(ITargetable obj) => OnFoundEvent?.Invoke(obj);
        public virtual void TargetLost(ITargetable obj, bool onDisable) => OnLostEvent?.Invoke(obj, onDisable);

        public abstract void Deactivate();
    }
}