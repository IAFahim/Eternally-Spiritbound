using _Root.Scripts.Model.Stats.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Boats.Runtime
{
    [CreateAssetMenu(fileName = "BoatVehicle", menuName = "Scriptable/Asset/Vehicles/Boat")]
    public class BoatVehicleAsset : VehicleAsset
    {
        public BoatControllerParameterScript boatControllerParameterScript;
    }
}