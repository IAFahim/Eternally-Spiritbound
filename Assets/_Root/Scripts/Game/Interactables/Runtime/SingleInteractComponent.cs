using System;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class SingleInteractComponent : MonoBehaviour
    {
        public PhysicsCheckOverlapNonAlloc playerOverlap;
        public IntervalTicker ticker;
        private IInteractableByGameObject _lastInteractedGameObject;
        public bool locked;

        private void Start()
        {
            playerOverlap.Initialize(1);
        }

        private void Update()
        {
            if (
                playerOverlap.foundSize > 0 &&
                playerOverlap.Colliders[0].gameObject.TryGetComponent(out _lastInteractedGameObject)
            )
            {
                _lastInteractedGameObject.OnInteractStart(gameObject);
                locked = true;
            }
            else if (locked)
            {
                _lastInteractedGameObject.OnInteractEnd(gameObject);
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