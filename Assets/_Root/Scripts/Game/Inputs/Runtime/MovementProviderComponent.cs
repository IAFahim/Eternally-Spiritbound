using _Root.Scripts.Model.Stats.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Inputs.Runtime
{
    [RequireComponent(typeof(EntityStatsComponent))]
    public abstract class MovementProviderComponent : MonoBehaviour, IMove
    {
        public EntityStatsComponent entityStatsComponent;
        [FormerlySerializedAs("isInputEnabled")]
        public bool inputEnabled;

        [ShowInInspector] [ReadOnly] protected Vector3 MoveDirection;
        [ShowInInspector] [ReadOnly] protected bool hasInputThisFrame;

        public bool IsInputEnabled
        {
            get => inputEnabled;
            set => inputEnabled = value;
        }

        public bool HasInputThisFrame
        {
            get => hasInputThisFrame;
            set => hasInputThisFrame = value;
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

        void IMoveInputConsumer.OnMoveInput(InputAction.CallbackContext context)
        {
            HasInputThisFrame = true;
            Move(context.ReadValue<Vector2>());
        }

        public virtual void Move(Vector2 direction)
        {
            MoveDirection = direction;
        }

        void IMoveInputConsumer.OnMoveInputCancel(InputAction.CallbackContext context) => OnMoveInputCancel(context);

        public void DisableMoveInput(InputActionReference moveAction)
        {
            moveAction.action.Disable();
            moveAction.action.performed -= ((IMoveInputConsumer)this).OnMoveInput;
            moveAction.action.canceled -= ((IMoveInputConsumer)this).OnMoveInputCancel;
        }

        protected virtual void OnMoveInputCancel(InputAction.CallbackContext context)
        {
            HasInputThisFrame = false;
            Move(Vector2.zero);
        }

        protected virtual void Reset()
        {
            entityStatsComponent = GetComponent<EntityStatsComponent>();
        }
    }
}