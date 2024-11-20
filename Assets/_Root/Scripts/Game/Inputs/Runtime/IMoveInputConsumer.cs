using UnityEngine;
using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    public interface IMoveInputConsumer : IInputConsumer
    {
        void EnableMoveInput(InputActionReference moveAction);
        void OnMoveInput(InputAction.CallbackContext context);
        void Move(Vector2 direction);
        void OnMoveInputCancel(InputAction.CallbackContext context);
        void DisableMoveInput(InputActionReference moveAction);
    }
}