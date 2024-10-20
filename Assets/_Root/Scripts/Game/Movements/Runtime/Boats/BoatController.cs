using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Movements.Runtime.Boats
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MovementProviderComponent, IMainCameraProvider
    {
        [Header("References")] public Rigidbody rb;
        public Lean lean;

        [Header("Movement Settings")] public float maxForwardSpeed = 100f;
        public float maxReverseSpeed = 100f;
        public float turnTorque = 10f;
        public float accelerationForce = 100f;
        public float reverseAccelerationForce = 100f;
        public float waterDrag = 0.99f;

        [Header("Buoyancy Settings")] public float waterLevel;
        public float buoyancyForce = 100f;
        public float waveIntensity = 5f;
        public float waveFrequency = 1.41f;

        [Header("Stability Settings")] public float stabilizationTorque = 5000f;
        private bool _isReversing;

        private Optional<Camera> _mainCamera;


        private void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdateLean();
        }

        private void FixedUpdate()
        {
            DetermineMovementDirection();
            ApplyAcceleration();
            ApplyTurning();
            ApplyBuoyancy();
            ApplyWaterDrag();
            StabilizeBoat();
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
            float targetAcceleration = _isReversing ? reverseAccelerationForce : accelerationForce;
            Vector3 forceDirection = transform.forward * targetAcceleration;
            rb.AddForce(forceDirection, ForceMode.Acceleration);

            // Limit speed based on direction
            float maxSpeed = _isReversing ? maxReverseSpeed : maxForwardSpeed;
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
                rb.AddTorque(torque * (turnTorque * turnMultiplier), ForceMode.Acceleration);
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
            float waterOffset = transform.position.y - waterLevel;
            float buoyancyMultiplier = Mathf.Clamp01(-waterOffset / 2);
            Vector3 buoyancyForceVector = Vector3.up * (buoyancyForce * buoyancyMultiplier);

            // Apply wave effect
            float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveIntensity;
            buoyancyForceVector += Vector3.up * waveOffset;

            rb.AddForce(buoyancyForceVector, ForceMode.Acceleration);
        }

        private void ApplyWaterDrag()
        {
            rb.linearVelocity *= waterDrag;
            rb.angularVelocity *= waterDrag;
        }

        public float dampingFactor = 5f; // Adjust this value for smoother stabilization

        private void StabilizeBoat()
        {
            // Get the current rotation in Euler angles
            Vector3 currentRotation = transform.eulerAngles;

            // Calculate the target rotation with zero X and Z tilt
            Quaternion targetRotation = Quaternion.Euler(0f, currentRotation.y, 0f);

            // Calculate the torque needed to reach the target rotation

            Vector3 stabilizingTorque = Vector3.Cross(transform.up, targetRotation * Vector3.up) * stabilizationTorque;
            stabilizingTorque -= rb.angularVelocity * dampingFactor;

            // Apply the stabilizing torque
            rb.AddTorque(stabilizingTorque);
        }

        public Camera MainCamera
        {
            set => _mainCamera = value;
        }
    }
}