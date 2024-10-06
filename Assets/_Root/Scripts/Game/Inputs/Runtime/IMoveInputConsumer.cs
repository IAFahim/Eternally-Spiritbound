using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    public interface IMoveInputConsumer : IInputConsumer
    {
        void EnableMoveInput(InputActionReference moveAction);
        void OnMoveInput(InputAction.CallbackContext context);
        void OnMoveInputCancel(InputAction.CallbackContext context);
        void DisableMoveInput(InputActionReference moveAction);
    }
}