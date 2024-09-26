using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Root.ShootEmUp
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputReader : MonoBehaviour
    {
        private PlayerInput playerInput;
        private InputAction moveAction;

        public Vector2 Move => moveAction.ReadValue<Vector2>();

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            FireAttack fierAttack = new FireAttack();
            moveAction = playerInput.actions["Move"];
        }
    }

    public class FireAttack : AttacK
    {
        public override void Attack<T>(T target)
        {
            Debug.Log("Attacking " + target.name);
        }
    }

    public abstract class AttacK
    {
        public abstract void Attack<T>(T target) where T : Component;
    }
}