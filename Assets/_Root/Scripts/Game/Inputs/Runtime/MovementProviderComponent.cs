using UnityEngine;
using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    public abstract class MovementProviderComponent : MonoBehaviour, IMove
    {
        public bool isInputEnabled;
        protected Vector3 MoveDirection;

        public bool IsInputEnabled
        {
            get => isInputEnabled;
            set => isInputEnabled = value;
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

        private void OnMoveInputCancel(InputAction.CallbackContext context) => MoveDirection = Vector3.zero;
    }
}