using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract bool DetectTarget(GameObject entity, out GameObject target);
    }
}