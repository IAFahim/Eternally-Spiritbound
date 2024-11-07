using _Root.Scripts.Game.Movements.Runtime.Boats;
using Soul.Interactables.Runtime;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public class BoatVehicleComponent : VehicleComponent
    {
        public BoatController boatController;

        public override void OnInteractionStarted(IInteractorEntryPoint interactorEntryPoint)
        {
            base.OnInteractionStarted(interactorEntryPoint);
            boatController.inputEnabled = true;
        }

        public override void OnInteractionEnded(IInteractorEntryPoint interactorEntryPoint)
        {
            base.OnInteractionEnded(interactorEntryPoint);
            boatController.inputEnabled = false;
        }
    }
}