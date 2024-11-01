using UnityEngine;

namespace _Root.Scripts.Game.Vehicles.Runtime
{
    public class DriverComponent : MonoBehaviour, IDriver
    {
        public bool disableGameObjectOnEnter = true;

        public void EnterVehicle(GameObject vehicleGameObject, Vector3 mountPosition)
        {
            if (disableGameObjectOnEnter) gameObject.SetActive(false);
            transform.SetParent(vehicleGameObject.transform);
            transform.localPosition = mountPosition;
        }

        public void ExitVehicle(GameObject vehicleGameObject, Vector3 exitPosition)
        {
            transform.localPosition = exitPosition;
            transform.SetParent(null);
            if (disableGameObjectOnEnter) gameObject.SetActive(true);
        }
    }
}