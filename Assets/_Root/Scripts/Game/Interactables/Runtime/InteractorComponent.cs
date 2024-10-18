using Sirenix.OdinInspector;
using Soul.OverlapSugar.Runtime;
using Soul.Tickers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Game.Interactables.Runtime
{
    public class InteractorComponent : MonoBehaviour, IInteractor
    {
        public PhysicsCheckOverlapNonAlloc playerOverlap;
        public IntervalTicker ticker;
        private IInteractable _lastInteractable;
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
                    _lastInteractable.OnHoverExit(this);
                    SetupInteractable(closestCollider);
                }
            }
            else if (_lastClosestCollider != null)
            {
                _lastInteractable.OnHoverExit(this);
                _lastClosestCollider = null;
                _lastInteractable = null;
            }
        }

        private void SetupInteractable(Collider closestCollider)
        {
            _lastClosestCollider = closestCollider;
            _lastInteractable = closestCollider.GetComponent<IInteractable>();
            _lastInteractable.OnInteractHoverEnter(this);
        }

        public GameObject GameObject => gameObject;

        [Button]
        public void Interact()
        {
            if (_lastInteractable == null) return;
            busy = true;
            _lastInteractable.OnInteractStart(this);
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