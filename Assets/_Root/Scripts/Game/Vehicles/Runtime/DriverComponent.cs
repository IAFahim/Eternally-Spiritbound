using _Root.Scripts.Model.Assets.Runtime;
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
            if (vehicleGameObject.TryGetComponent<IAssetScriptStorageReference>(out var vehicleStorageReference))
            {
                if (TryGetComponent<IAssetScriptStorageReference>(out var driverStorageReference))
                {
                    driverStorageReference.AssetScriptStorage.MergeFrom(driverStorageReference.AssetScriptStorage);
                }
            }
            if (disableGameObjectOnEnter) gameObject.SetActive(true);
        }
    }
}