using System;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public interface ITargeter
    {
        bool HasTarget { get; }
        ITargetable CurrentTarget { get; }
        Transform Transform { get; }
        event Action<ITargetable> OnTargetFound;
        event Action<ITargetable, bool> OnTargetLost;
        void RemoveTarget(ITargetable targetable, bool onDisable);
    }
}