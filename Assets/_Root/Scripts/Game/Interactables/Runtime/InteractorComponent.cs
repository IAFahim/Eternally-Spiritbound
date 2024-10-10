using Pancake;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractorComponent : GameComponent
    {
        public PhysicsCheckOverlapNonAlloc playerOverlap;
        public IntervalTicker ticker;
        private IInteractableByGameObject _lastInteractedGameObject;
        public bool locked;

        private void Start()
        {
            playerOverlap.Initialize();
        }

        private void Update()
        {
            if (
                playerOverlap.currentSize > 0 &&
                playerOverlap.TryGetClosest(out Collider closestCollider, out _) &&
                closestCollider.gameObject.TryGetComponent(out _lastInteractedGameObject)
            )
            {
                locked = _lastInteractedGameObject.CanInteract(GameObject);
            }
            else if (locked)
            {
                _lastInteractedGameObject.OnInteractExit(GameObject);
                locked = false;
            }
        }

        private void FixedUpdate()
        {
            if (ticker.TryTick()) playerOverlap.Perform(out _);
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            playerOverlap.DrawGizmos(Color.red, Color.green);
        }
#endif
    }
}