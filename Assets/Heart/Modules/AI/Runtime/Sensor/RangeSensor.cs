using System.Collections.Generic;
using Pancake.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pancake.AI
{
    public class RangeSensor : Sensor
    {
        [Space(8)] [SerializeField] private float radius = 1f;

        [Space(8)] [SerializeField] private bool stopAfterFirstHit;
        [SerializeField] private bool detectOnStart = true;
#if UNITY_EDITOR
        [SerializeField] private bool showGizmos = true;
#endif
        [Space(8), SerializeField, Required] private Transform center;

        [SerializeField, Required] private Transform source;

        [SerializeField] private GameObjectUnityEvent detectedEvent;

        private readonly Collider[] _hits = new Collider[16];
        private readonly HashSet<Collider> _hitObjects = new();
        private int _frames;
        private int _count;

        private void Awake()
        {
            if (detectOnStart) Pulse();
        }

        public override void Pulse()
        {
            _hitObjects.Clear();
            isPlaying = true;
        }

        protected void FixedUpdate()
        {
            if (!isPlaying) return;
            _frames++;
            if (_frames % raycastRate != 0) return;
            _frames = 0;
            Procedure();
        }

        private void Procedure()
        {
            var currentPosition = source.TransformPoint(center.localPosition);
            Raycast(currentPosition);
        }

        private void Raycast(Vector3 center)
        {
            _count = Physics.OverlapSphereNonAlloc(center, radius, _hits, layer);
            if (_count <= 0) return;
            for (var i = 0; i < _count; i++)
            {
                var hit = _hits[i];
                if (hit != null && hit.transform != source) HandleHit(hit);
            }
        }

        private void HandleHit(Collider hit)
        {
            if (_hitObjects.Contains(hit)) return;
            _hitObjects.Add(hit);
            detectedEvent?.Invoke(hit.gameObject);
            if (stopAfterFirstHit) Stop();

#if UNITY_EDITOR
            if (showGizmos)
            {
                Debug.DrawRay(hit.transform.position, Vector2.down * 0.4f, Color.red, 0.6f);
                Debug.DrawRay(hit.transform.position, Vector2.right * 0.4f, Color.red, 0.6f);
            }
#endif
        }

        public Transform GetClosestTarget()
        {
            if (_count == 0) return null;
            
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = source.TransformPoint(center.localPosition);
            // todo
            return null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (center != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(center.position, 0.1f);

                if (showGizmos)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(center.position, radius);
                }
            }
        }
#endif
    }
}