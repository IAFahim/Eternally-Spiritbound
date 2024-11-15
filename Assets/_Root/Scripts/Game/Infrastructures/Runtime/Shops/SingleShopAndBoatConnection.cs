using _Root.Scripts.Model.Assets.Runtime;
using Cysharp.Threading.Tasks;
using Pancake.Pools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public class SingleShopAndBoatConnection : MonoBehaviour
    {
        public Vector3 offset;
        public Quaternion rotation;

        public float positionForceMultiplier = 1f;
        public float rotationTorqueMultiplier = 1f;

        private GameObject _boat;
        private Rigidbody _boatRigidbody;

        public async UniTaskVoid SpawnBoat(AssetScript assetScript)
        {
            var transformPoint = transform.position + offset;
            _boat = await assetScript.AssetReference.RequestAsync(transformPoint, rotation);
            _boatRigidbody = _boat.GetComponent<Rigidbody>();
        }

        public void DespawnBoat(AssetScript assetScript) => assetScript.AssetReference.Return(_boat);

        private void PlaceAndAlignViaRigidBody()
        {
            // Calculate the desired position and rotation
            Vector3 desiredPosition = transform.position + offset;
            Quaternion desiredRotation = rotation;

            // Calculate the force needed to move the boat to the desired position
            Vector3 positionDelta = desiredPosition - _boatRigidbody.position;
            Vector3 force = positionDelta * _boatRigidbody.mass / Time.fixedDeltaTime * positionForceMultiplier;

            // Apply the force to the boat's rigidbody
            _boatRigidbody.AddForce(force, ForceMode.Force);

            // Calculate the torque needed to rotate the boat to the desired rotation
            Quaternion rotationDelta = desiredRotation * Quaternion.Inverse(_boatRigidbody.rotation);
            rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180f) angle -= 360f;
            Vector3 torque = axis.normalized * (angle * Mathf.Deg2Rad * _boatRigidbody.mass / Time.fixedDeltaTime *
                                                rotationTorqueMultiplier);

            // Apply the torque to the boat's rigidbody
            _boatRigidbody.AddTorque(torque, ForceMode.Force);
        }

        [Button]
        private void GetBoatPositionAsOffset()
        {
            offset = _boat.transform.position - transform.position;
        }

        [Button]
        private void GetBoatRotation()
        {
            rotation = _boat.transform.rotation;
        }

        private void FixedUpdate()
        {
            PlaceAndAlignViaRigidBody();
        }

        private void OnDrawGizmos()
        {
            Vector3 desiredPosition = transform.position + offset;
            Quaternion desiredRotation = rotation;

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(desiredPosition, .2f);

            Gizmos.color = Color.red;
            Vector3 forwardDirection = desiredRotation * Vector3.forward;
            Gizmos.DrawLine(desiredPosition, desiredPosition + forwardDirection * 2);
        }
    }
}