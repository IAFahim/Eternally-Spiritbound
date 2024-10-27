using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public class OnTriggerActivator : MonoBehaviour
    {
        [SerializeField] private ActivatorScript activatorScript;

        private void OnTriggerEnter(Collider other)
        {
            activatorScript.Activate(other);
        }

        private void OnTriggerExit(Collider other)
        {
            activatorScript.Deactivate(other);
        }
    }
}