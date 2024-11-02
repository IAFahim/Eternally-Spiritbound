using _Root.Scripts.Game.Ai.Runtime.Targets;
using Sirenix.OdinInspector;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using Color = UnityEngine.Color;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public class TargetInBoundActivatorComponent : MonoBehaviour
    {
        [SerializeField] private ActivatorScript activatorScript;
        [SerializeField] private bool isInside;
        [SerializeField] private Bounds[] bounds;

        [FormerlySerializedAs("targetingStrategy")] [SerializeField]
        private TargetStrategy targetStrategy;

        [SerializeField] private IntervalTicker checkInterval;

        [ShowInInspector] private bool _targetFound;
        private ITargetable _targetable;

        private void OnEnable()
        {
            targetStrategy.Start();
            targetStrategy.Register(null, OnTargetFound, OnTargetLost);
        }

        private void OnTargetLost(ITargetable _, bool onDisable)
        {
            ClearTarget();
        }

        private void ClearTarget()
        {
            _targetFound = false;
            _targetable = null;
        }

        private void OnTargetFound(ITargetable targetable)
        {
            _targetFound = true;
            _targetable = targetable;
        }


        private void Update()
        {
            if (checkInterval.TryTick()) CheckForTargets();
        }

        private void CheckForTargets()
        {
            if (!_targetFound) return;
            ProcessBounds();
        }

        private void ProcessBounds()
        {
            var targetTransform = _targetable.Transform;
            var position = targetTransform.position;
            foreach (var bound in bounds)
            {
                if (!bound.Contains(position)) continue;
                if (isInside) return;
                activatorScript.Activate(targetTransform);
                targetStrategy.Register(null, OnTargetFound, OnTargetLost);
                isInside = true;
                return;
            }

            if (isInside)
            {
                activatorScript.Deactivate(targetTransform);
                isInside = false;
            }
        }

        private void OnDisable()
        {
            targetStrategy.UnRegister(null, OnTargetFound, OnTargetLost);
            if (_targetFound && _targetable != null && _targetable.Transform != null)
            {
                activatorScript.Deactivate(_targetable.Transform);
            }

            activatorScript.CleanUp();
            ClearTarget();
        }


        [Button]
        private void GenerateBounds()
        {
            var boxColliders = GetComponents<BoxCollider>();
            if (boxColliders.Length == 0) return;
            bounds = new Bounds[boxColliders.Length];
            for (int i = 0; i < boxColliders.Length; i++)
            {
                bounds[i] = boxColliders[i].bounds;
                if (Application.isEditor) DestroyImmediate(boxColliders[i]);
            }
        }


        [Button]
        public void GenerateBoundingBox()
        {
            if (bounds == null || bounds.Length == 0) return;
            var boxColliders = GetComponents<BoxCollider>();
            if (Application.isEditor)
            {
                foreach (var boxCollider in boxColliders) DestroyImmediate(boxCollider);
            }

            foreach (var bound in bounds)
            {
                var boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.center = transform.InverseTransformPoint(bound.center);
                boxCollider.size = bound.size;
                boxCollider.isTrigger = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (bounds == null || bounds.Length == 0) return;
            Gizmos.color = isInside ? Color.green : Color.red;
            foreach (var bound in bounds)
            {
                Gizmos.DrawWireCube(bound.center, bound.size);
            }
        }
    }
}