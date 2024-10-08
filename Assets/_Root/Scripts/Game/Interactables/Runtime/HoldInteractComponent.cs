using Soul.OverlapSugar.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class HoldInteractComponent : MonoBehaviour
    {
        public LayerMask playerLayer;
        public bool vibrateOnRange = true;
        public PhysicsCheckOverlapNonAlloc playerOverlap;
        private IInteract _lastInteract;

        private void Start()
        {
            playerOverlap.Initialize(1);
        }

        private void FixedUpdate()
        {
            playerOverlap.Perform();
        }
    }
}