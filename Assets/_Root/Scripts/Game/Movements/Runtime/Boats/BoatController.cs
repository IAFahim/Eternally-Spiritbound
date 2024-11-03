using System;
using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.Interactables.Runtime;
using _Root.Scripts.Model.Boats.Runtime;
using _Root.Scripts.Model.Water.Runtime;
using Pancake;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Movements.Runtime.Boats
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MovementProviderComponent, IMainCameraProvider
    {
        [Header("References")] public Rigidbody rb;
        [SerializeField] private Lean lean;
        [SerializeField] private BoatControllerParameterScript parameterScript;

        [ShowInInspector, NonSerialized, ReadOnly]
        public BoatControllerParameters Parameters;

        [SerializeField] private WaterParameterScript waterParameterScript;
        private bool _isReversing;
        private Optional<Camera> _mainCamera;


        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            Parameters = parameterScript.value;
        }

        private void Update()
        {
            UpdateLean();
        }

        private void FixedUpdate()
        {
            ApplyBuoyancy();
            StabilizeBoat();
            if (!IsInputEnabled) return;
            DetermineMovementDirection();
            ApplyAcceleration();
            ApplyTurning();
            ApplyWaterDrag();
        }


        protected override void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 input = IsInputEnabled ? context.ReadValue<Vector2>() : Vector2.zero;

            if (_mainCamera.Enabled)
            {
                // Convert input to camera-relative direction
                Vector3 cameraForward = _mainCamera.Value.transform.forward;
                Vector3 cameraRight = _mainCamera.Value.transform.right;

                // Project vectors onto the horizontal plane
                cameraForward.y = 0;
                cameraRight.y = 0;

                cameraForward.Normalize();
                cameraRight.Normalize();

                // Calculate the final direction based on camera orientation
                Vector3 moveDir = (cameraRight * input.x + cameraForward * input.y).normalized;
                MoveDirection = moveDir;
            }
            else MoveDirection = new Vector3(input.x, 0, input.y).normalized;
        }

        private void OnMoveInputCancel(InputAction.CallbackContext context)
        {
            MoveDirection = Vector3.zero;
        }

        private void UpdateLean()
        {
            lean.UpdateLean(MoveDirection);
        }

        private void DetermineMovementDirection()
        {
            float dotProduct = Vector3.Dot(rb.linearVelocity, transform.forward);
            _isReversing = dotProduct < 0;
        }

        private void ApplyAcceleration()
        {
            float targetAcceleration =
                _isReversing ? Parameters.reverseAccelerationForce : Parameters.accelerationForce;
            Vector3 forceDirection = transform.forward * targetAcceleration;
            rb.AddForce(forceDirection, ForceMode.Acceleration);

            // Limit speed based on direction
            float maxSpeed = _isReversing ? Parameters.maxReverseSpeed : Parameters.maxForwardSpeed;
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        private void ApplyTurning()
        {
            if (MoveDirection.magnitude > 0)
            {
                // Calculate the target rotation based on input
                Quaternion targetRotation = Quaternion.LookRotation(MoveDirection, Vector3.up);

                // Apply torque to rotate towards the target rotation
                Vector3 torque = CalculateTorqueToTarget(targetRotation);
                float turnMultiplier = _isReversing ? 0.5f : 1f; // Reduce turning while reversing
                rb.AddTorque(torque * (Parameters.turnTorque * turnMultiplier), ForceMode.Acceleration);
            }
        }

        private Vector3 CalculateTorqueToTarget(Quaternion targetRotation)
        {
            Quaternion rotationDifference = targetRotation * Quaternion.Inverse(transform.rotation);
            rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f)
                angle -= 360f;

            return axis.normalized * (angle * Mathf.Deg2Rad);
        }

        private void ApplyBuoyancy()
        {
            float waterOffset = transform.position.y - waterParameterScript.value.waterLevel;
            float buoyancyMultiplier = Mathf.Clamp01(-waterOffset / 2);
            Vector3 buoyancyForceVector = Vector3.up * (waterParameterScript.value.buoyancyForce * buoyancyMultiplier);

            // Apply wave effect
            float waveOffset = waterParameterScript.WaveOffset;
            buoyancyForceVector += Vector3.up * waveOffset;

            rb.AddForce(buoyancyForceVector, ForceMode.Acceleration);
        }

        private void ApplyWaterDrag()
        {
            rb.linearVelocity *= waterParameterScript.value.waterDrag;
            rb.angularVelocity *= waterParameterScript.value.waterDrag;
        }


        private void StabilizeBoat()
        {
            // Get the current rotation in Euler angles
            Vector3 currentRotation = transform.eulerAngles;

            // Calculate the target rotation with zero X and Z tilt
            Quaternion targetRotation = Quaternion.Euler(0f, currentRotation.y, 0f);

            // Calculate the torque needed to reach the target rotation

            Vector3 stabilizingTorque = Vector3.Cross(transform.up, targetRotation * Vector3.up) *
                                        Parameters.stabilizationTorque;
            stabilizingTorque -= rb.angularVelocity * Parameters.dampingFactor;

            // Apply the stabilizing torque
            rb.AddTorque(stabilizingTorque);
        }

        public Camera MainCamera
        {
            set => _mainCamera = value;
        }
    }
}