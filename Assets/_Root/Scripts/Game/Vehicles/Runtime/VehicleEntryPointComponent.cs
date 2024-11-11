using _Root.Scripts.Game.Interactables.Runtime;
using Soul.Interactables.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public class VehicleEntryPointComponent : InteractableEntryPointComponent
    {
        public DriverComponent driverComponent;
        public Vector3 mountPosition;
        public Vector3 exitPosition;

        public override void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint)
        {
            base.OnInteractionStarted(interactorEntryPoint);
            interactorEntryPoint.GameObject.GetComponent<IDriver>().EnterVehicle(gameObject, mountPosition);
        }

        public override void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint)
        {
            base.OnInteractionEnded(interactorEntryPoint);
            interactorEntryPoint.GameObject.GetComponent<IDriver>().ExitVehicle(gameObject, exitPosition);
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(mountPosition, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(exitPosition, 0.1f);
        }
#endif
    }
}