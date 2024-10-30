using System;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public interface ITargeter
    {
        Transform Transform { get; }
        public event Action<ITargetable> OnTargetFound;
        public event Action<ITargetable, bool> OnTargetLost;
        public void RemoveTarget(ITargetable targetable, bool onDisable);
    }
}