using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public interface ITargeter
    {
        Transform Transform { get; }
        TargetingStrategy TargetingStrategy { get; }
    }
}