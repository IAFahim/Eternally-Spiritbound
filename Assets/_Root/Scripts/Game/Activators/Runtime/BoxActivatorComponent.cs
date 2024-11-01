using System.Collections;
using _Root.Scripts.Game.Ai.Runtime.Targets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public class BoxActivatorComponent : MonoBehaviour
    {
        [SerializeField] private ActivatorScript activatorScript;
        [SerializeField] private bool isInside;
        [SerializeField] private Bounds[] bounds;

        [SerializeField] private TargetingStrategy targetingStrategy;
        [SerializeField] private float checkInterval = 1f;
        private ITargetable _targetable;

        private WaitForSeconds _waitForSeconds;

        private void OnEnable()
        {
            if (!targetingStrategy.TryGetTarget(null, out _targetable))
            {
                targetingStrategy.OnFoundEvent += TargetingStrategyOnFoundEvent;
                targetingStrategy.StartTargetLookup();
            }

            _waitForSeconds = new WaitForSeconds(checkInterval);
            StartCoroutine(CheckForTargets());
        }

        private void TargetingStrategyOnFoundEvent(ITargetable targetable) => _targetable = targetable;

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

        private IEnumerator CheckForTargets()
        {
            while (true)
            {
                if (_targetable == null) yield return _waitForSeconds;
                else
                {
                    if (IsInBounds()) yield break;
                    yield return _waitForSeconds;
                }
            }
        }

        private bool IsInBounds()
        {
            var targetTransform = _targetable.Transform;
            var position = targetTransform.position;
            foreach (var bound in bounds)
            {
                if (!bound.Contains(position)) continue;
                activatorScript.Activate(targetTransform);
                return isInside = true;
            }

            if (isInside)
            {
                activatorScript.Deactivate(targetTransform);
                isInside = false;
            }

            return false;
        }


        private void OnDisable()
        {
            targetingStrategy.StopTargetLookup();
            targetingStrategy.OnFoundEvent -= TargetingStrategyOnFoundEvent;
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