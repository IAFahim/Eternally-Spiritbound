using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public interface ITargetable
    {
        Transform Transform { get; }
        public void AddTargeter(ITargeter targeter);
        public void RemoveTargeter(ITargeter targeter);
    }
}