using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Movements
{
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour
    {
        public Rigidbody rb;
        public float speed = 10f;
        public float turnSpeed = 10f;
        public float force = 10f;
        public float acceleration = 10f;
        public float waveIntensity = 0.5f; // Adjust wave intensity
        public float waveFrequency = 1f; // Adjust wave frequency

        public InputActionReference moveAction;
        public InputActionReference accelerateAction;

        public Lean lean;
        private Vector3 _moveDirection;
        [SerializeField] private float _accelerationInput;


        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();

            moveAction.action.Enable();
            moveAction.action.performed += MoveInput;
            moveAction.action.canceled += MoveInputCancel;

            accelerateAction.action.Enable();
            accelerateAction.action.performed += AccelerateInput;
            accelerateAction.action.canceled += AccelerateInput;
        }

        private void MoveInputCancel(InputAction.CallbackContext obj)
        {
            _moveDirection = Vector3.zero;
        }

        void OnDisable()
        {
            moveAction.action.Disable();
            moveAction.action.performed -= MoveInput;
            moveAction.action.canceled -= MoveInputCancel;

            accelerateAction.action.Disable();
            accelerateAction.action.performed -= AccelerateInput;
            accelerateAction.action.canceled -= AccelerateInput;
        }

        private void MoveInput(InputAction.CallbackContext obj)
        {
            var vector2 = obj.ReadValue<Vector2>();
            _moveDirection = new Vector3(vector2.x, 0, vector2.y);
        }

        private void AccelerateInput(InputAction.CallbackContext obj)
        {
            // _accelerationInput = obj.ReadValue<float>();
            _accelerationInput = 1;
        }

        private void Update()
        {
            lean.UpdateLean(_moveDirection);
            _accelerationInput = Mathf.Lerp(_accelerationInput, acceleration * _moveDirection.magnitude, Time.deltaTime);
        }

        void FixedUpdate()
        {
            // Apply acceleration force
            if (_accelerationInput > 0)
            {
                rb.AddForce(transform.forward * (force * _accelerationInput), ForceMode.Acceleration);
            }

            // Apply turning force
            if (_moveDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            // Apply drag to slow down the boat when not accelerating
            rb.linearVelocity *= (1f - Time.deltaTime * rb.linearDamping);
        }
    }
}