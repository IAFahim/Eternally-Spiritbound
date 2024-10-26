using UnityEngine;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public class DriverComponent : MonoBehaviour, IDriver
    {
        public void EnterVehicle(GameObject vehicleGameObject, Vector3 mountPosition)
        {
            transform.position = mountPosition;
            transform.SetParent(vehicleGameObject.transform);
        }

        public void ExitVehicle(GameObject vehicleGameObject, Vector3 exitPosition)
        {
            transform.position = exitPosition;
            transform.SetParent(null);
        }
    }
}