using System;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public event Action<ITargetable> OnFoundEvent;
        public event Action<ITargetable> OnLostEvent;

        public abstract void Initialize();
        public abstract bool TryGetTarget(out ITargetable targetable);
        
        public virtual void TargetFound(ITargetable obj) => OnFoundEvent?.Invoke(obj);
        public virtual void TargetLost(ITargetable obj) => OnLostEvent?.Invoke(obj);

        public abstract void CleanUp();
    }
}