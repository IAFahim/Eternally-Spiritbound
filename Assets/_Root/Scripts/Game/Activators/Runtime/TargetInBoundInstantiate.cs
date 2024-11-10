using _Root.Scripts.Game.Ai.Runtime.Targets;
using Sirenix.OdinInspector;
using Soul.Tickers.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Color = UnityEngine.Color;

namespace _Root.Scripts.Game.Activators.Runtime
{
    public class TargetInBoundInstantiate : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject asset;
        [SerializeField] private bool isInside;
        [SerializeField] private Bounds[] bounds;

        [FormerlySerializedAs("targetingStrategy")] [SerializeField]
        private TargetStrategy targetStrategy;


        [SerializeField] private IntervalTicker checkInterval;

        private ITargetable _targetable;
        private GameObject _currentInstance;

        private void OnEnable()
        {
            targetStrategy.Stop();
            targetStrategy.Start();
            targetStrategy.Register(null, OnTargetFound, OnTargetLost);
        }

        private void OnTargetLost(ITargetable _, bool onDisable)
        {
            ClearTarget();
        }

        private void ClearTarget()
        {
            _targetable = null;
        }

        private void OnTargetFound(ITargetable targetable)
        {
            _targetable = targetable;
        }


        private void Update()
        {
            if (checkInterval.TryTick()) CheckForTargets();
        }

        private void CheckForTargets()
        {
            if (_targetable != null) ProcessBounds();
        }

        private void ProcessBounds()
        {
            var targetTransform = _targetable.Transform;
            var position = targetTransform.position;
            foreach (var bound in bounds)
            {
                if (!bound.Contains(position)) continue;
                if (isInside) return;
                Addressables.InstantiateAsync(asset, targetTransform.position, Quaternion.identity).Completed +=
                    OnInstantiate;
                targetStrategy.Register(null, OnTargetFound, OnTargetLost);
                isInside = true;
                return;
            }

            if (isInside)
            {
                Addressables.ReleaseInstance(_currentInstance);
                isInside = false;
            }
        }

        private void OnInstantiate(AsyncOperationHandle<GameObject> obj) => _currentInstance = obj.Result;

        private void OnDisable()
        {
            targetStrategy.UnRegister(null, OnTargetFound, OnTargetLost);
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