using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public class OnTriggerActivator : MonoBehaviour
    {
        private ActivatorScript _activatorScript;

        private void OnTriggerEnter(Collider other)
        {
            _activatorScript.Activate(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _activatorScript.Deactivate(other);
        }
    }
}