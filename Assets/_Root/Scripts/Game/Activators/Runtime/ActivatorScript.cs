using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public abstract class ActivatorScript : ScriptableObject
    {
        public abstract void Activate(Transform activatorInvoker);

        public abstract void Deactivate(Transform activatorInvoker);
    }
}