using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    public abstract class MovementProviderComponent : MonoBehaviour, IMove
    {
        [FormerlySerializedAs("isInputEnabled")]
        public bool inputEnabled;

        protected Vector3 MoveDirection;

        public bool IsInputEnabled
        {
            get => inputEnabled;
            set => inputEnabled = value;
        }

        public Vector3 Direction
        {
            get => MoveDirection;
            set => MoveDirection = value;
        }

        public void EnableMoveInput(InputActionReference moveAction)
        {
            moveAction.action.Enable();
            moveAction.action.performed += ((IMoveInputConsumer)this).OnMoveInput;
            moveAction.action.canceled += ((IMoveInputConsumer)this).OnMoveInputCancel;
        }

        void IMoveInputConsumer.OnMoveInput(InputAction.CallbackContext context) => OnMoveInput(context);

        protected abstract void OnMoveInput(InputAction.CallbackContext context);

        void IMoveInputConsumer.OnMoveInputCancel(InputAction.CallbackContext context) => OnMoveInputCancel(context);

        public void DisableMoveInput(InputActionReference moveAction)
        {
            moveAction.action.Disable();
            moveAction.action.performed -= ((IMoveInputConsumer)this).OnMoveInput;
            moveAction.action.canceled -= ((IMoveInputConsumer)this).OnMoveInputCancel;
        }

        protected virtual void OnMoveInputCancel(InputAction.CallbackContext context) => MoveDirection = Vector3.zero;
    }
}