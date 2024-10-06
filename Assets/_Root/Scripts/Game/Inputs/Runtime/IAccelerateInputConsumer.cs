using UnityEngine.InputSystem;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    public interface IAccelerateInputConsumer : IInputConsumer
    {
        void EnableAccelerateInput(InputActionReference accelerateAction);
        void OnAccelerateInput(InputAction.CallbackContext context);
        void OnAccelerateInputCancel(InputAction.CallbackContext context);
        void DisableAccelerateInput(InputActionReference accelerateAction);
    }
}