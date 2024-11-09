using _Root.Scripts.Model.Assets.Runtime;
using Sirenix.OdinInspector;
using Soul.Pools.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Infrastructures.Runtime.Shops
{
    public class SingleShopAndBoatConnection : MonoBehaviour
    {
        public AssetBase boatAsset;
        public Vector3 offset;
        public Quaternion rotation;
        public ScriptablePool scriptablePool;
        public GameObject boat;
        
        public float positionForceMultiplier = 1f;
        public float rotationTorqueMultiplier = 1f;
        
        private Rigidbody _boatRigidbody;

        private void Awake()
        {
            var transformPoint= transform.TransformPoint(offset);
            boat = scriptablePool.Request(boatAsset.assetReferenceGameObject, transformPoint, rotation);
            _boatRigidbody = boat.GetComponent<Rigidbody>();
        }

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
            offset = boat.transform.position - transform.position;
        }

        [Button]
        private void GetBoatRotation()
        {
            rotation = boat.transform.rotation;
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