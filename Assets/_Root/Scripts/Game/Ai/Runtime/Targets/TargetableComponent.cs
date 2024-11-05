using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Targets
{
    public class TargetableComponent : MonoBehaviour, ITargetable
    {
        public Transform Transform => transform;
        [ShowInInspector][ReadOnly] private readonly HashSet<ITargeter> _targeters = new();

        public void AddTargeter(ITargeter targeter)
        {
            _targeters.Add(targeter);
        }

        public void RemoveTargeter(ITargeter targeter)
        {
            _targeters.Remove(targeter);
        }

        private void OnDisable()
        {
            foreach (var targeter in _targeters)
            {
                targeter.RemoveTarget(this, true);
            }
            _targeters.Clear();
        }
    }
}