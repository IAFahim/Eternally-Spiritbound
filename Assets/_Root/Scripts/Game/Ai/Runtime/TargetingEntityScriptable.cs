using System.Collections.Generic;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public class TargetingEntityScriptable : ScriptableObject
    {
        private readonly Dictionary<GameObject, GameObject> _entityTargets = new();
        
        public void AddTarget(GameObject entity, GameObject target) => _entityTargets.TryAdd(entity, target);

        public void RemoveTarget(GameObject entity) => _entityTargets.Remove(entity);
        public void ClearTargets() => _entityTargets.Clear();
        
    }
}