using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public abstract class TargetStrategy : ScriptableObject
    {
        protected bool IsActive;
        protected Action<ITargetable> FoundCallBack;
        protected Action<ITargetable, bool> LostCallback;

        public abstract void Start();
        public abstract void Set(ITargetable targetable);
        public abstract void Remove(ITargetable targetable, bool onDisable);

        public abstract void Register([CanBeNull] ITargeter targeter, Action<ITargetable> onTargetFound,
            Action<ITargetable, bool> onTargetLost);

        public abstract void UnRegister([CanBeNull] ITargeter targeter, Action<ITargetable> onTargetFound,
            Action<ITargetable, bool> onTargetLost);

        public abstract void Stop();
    }
}