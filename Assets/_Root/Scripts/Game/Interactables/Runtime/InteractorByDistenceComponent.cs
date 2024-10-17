using System;
using Sirenix.OdinInspector;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractorByDistenceComponent : MonoBehaviour, IInteractorGameObject
    {
        public event Action<IInteractableWithGameObject> OnInteractorFound;
        public PhysicsCheckOverlapNonAlloc playerOverlap;
        public IntervalTicker ticker;
        private IInteractableWithGameObject _lastInteractableWithGameObject;
        public bool busy;
        public Collider _lastClosestCollider;

        private void Start()
        {
            playerOverlap.Initialize();
        }

        private void Update()
        {
            if (busy) return;
            if (playerOverlap.TryGetClosest(out var closestCollider, out _))
            {
                if (_lastClosestCollider == null) SetupInteractable(closestCollider);
                else if (_lastClosestCollider != closestCollider)
                {
                    _lastInteractableWithGameObject.OnHoverExit(gameObject);
                    SetupInteractable(closestCollider);
                }
            }
            else if (_lastClosestCollider != null)
            {
                _lastInteractableWithGameObject.OnHoverExit(gameObject);
                _lastClosestCollider = null;
                _lastInteractableWithGameObject = null;
            }
        }

        private void SetupInteractable(Collider closestCollider)
        {
            _lastClosestCollider = closestCollider;
            _lastInteractableWithGameObject = closestCollider.GetComponent<IInteractableWithGameObject>();
            _lastInteractableWithGameObject.OnInteractHoverEnter(gameObject);
            OnInteractorFound?.Invoke(_lastInteractableWithGameObject);
        }

        [Button]
        public void Interact()
        {
            if (_lastInteractableWithGameObject == null) return;
            busy = true;
            _lastInteractableWithGameObject.OnInteractStart(gameObject, () => busy = false);
        }

        private void FixedUpdate()
        {
            if (!busy && ticker.TryTick()) playerOverlap.Perform();
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            playerOverlap.DrawGizmos(Color.red, Color.green);
        }
#endif
    }
}