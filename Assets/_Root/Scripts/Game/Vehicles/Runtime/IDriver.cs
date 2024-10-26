using UnityEngine;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public interface IDriver
    {
        public void EnterVehicle(GameObject vehicleGameObject, Vector3 mountPosition);
        public void ExitVehicle(GameObject vehicleGameObject, Vector3 exitPosition);
    }
}