using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public abstract class ActivatorScript : ScriptableObject
    {
        public abstract void Activate(Collider other);

        public abstract void Deactivate(Collider other);
    }
}