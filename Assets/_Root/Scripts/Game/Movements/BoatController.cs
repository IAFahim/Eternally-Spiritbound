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
        public float force = 10f; // Renamed horsePower to force for clarity

        public InputActionReference moveAction;
        public InputActionReference accelerateAction; // New input for acceleration

        private Vector3 _moveDirection;
        private float _accelerationInput;

        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            // Rigidbody Setup:
            rb.mass = 1000f; // Adjust as needed for boat weight
            rb.linearDamping = 1f; // Adjust for water resistance
            rb.angularDamping = 2f; // Adjust for rotational resistance

            moveAction.action.Enable();
            moveAction.action.performed += MoveInput;
            moveAction.action.canceled += MoveInput;

            accelerateAction.action.Enable();
            accelerateAction.action.performed += AccelerateInput;
            accelerateAction.action.canceled += AccelerateInput;
        }

        void OnDisable()
        {
            moveAction.action.Disable();
            moveAction.action.performed -= MoveInput;
            moveAction.action.canceled -= MoveInput;

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
            _accelerationInput = obj.ReadValue<float>();
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
            rb.linearVelocity *= (1f - Time.deltaTime * rb.linearDamping); // Using rb.drag for a more realistic effect
        }
    }
}