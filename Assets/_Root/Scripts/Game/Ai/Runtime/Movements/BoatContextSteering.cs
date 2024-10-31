using _Root.Scripts.Game.Ai.Runtime.Targets;
using _Root.Scripts.Game.Inputs.Runtime;
using Sisus.Init;
using UnityEngine;

namespace _Root.Scripts.Game.Ai.Runtime.Movements
{
    public class BoatContextSteering : MonoBehaviour<BoatContextConfig, ITargeter>
    {
        public BoatContextConfig config;

        private ITargeter _targeter;
        private IMove _move;
        private float[] _interestArray;
        private float[] _dangerArray;
        private Vector3[] _directionVectors;
        private Vector3 _resultDirection;
        private Vector3 _lastNonZeroDirection;


        protected override void Init(BoatContextConfig firstArgument, ITargeter secondArgument)
        {
            config = firstArgument;
            _targeter = secondArgument;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _move = GetComponent<IMove>();
        }

        private void Start()
        {
            InitializeArrays();
            config.Initialize(transform);
        }

        private void InitializeArrays()
        {
            _interestArray = new float[config.directions];
            _dangerArray = new float[config.directions];
            _directionVectors = new Vector3[config.directions];

            for (int i = 0; i < config.directions; i++)
            {
                float angle = i * (360f / config.directions);
                _directionVectors[i] = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            }
        }

        public Vector3 Steer(Vector3 position)
        {
            CheckAndProcessObstacle();
            CalculateInterest(position);
            return CalculateResultDirection();
        }

        private void ClearArrays()
        {
            for (int i = 0; i < config.directions; i++)
            {
                _interestArray[i] = 0;
                _dangerArray[i] = 0;
            }
        }

        public void Update()
        {
            var hasTarget = _targeter.HasTarget;
            _move.IsInputEnabled = hasTarget;
            if (hasTarget) _move.Direction = Steer(_targeter.CurrentTarget.Transform.position);
        }

        public void FixedUpdate()
        {
            if (!config.ticker.TryTick()) return;
            ClearArrays();
            config.obstacleDetector.Perform();
        }


        private void CheckAndProcessObstacle()
        {
            if (!config.obstacleDetector.Found()) return;
            int colliderCount = config.obstacleDetector.GetColliders(out var colliders);
            for (var i = 0; i < colliderCount; i++) ProcessObstacle(colliders[i]);
        }

        private void ProcessObstacle(Collider obstacle)
        {
            Vector3 toObstacle = obstacle.ClosestPoint(transform.position) - transform.position;
            toObstacle.y = 0; // Keep navigation on XZ plane
            float distance = toObstacle.magnitude;

            if (distance < 0.1f) return; // Prevent division by zero

            Vector3 directionToObstacle = toObstacle / distance;
            float dangerWeight = Mathf.Clamp01(1 - (distance / config.dangerDecayDistance));

            for (int i = 0; i < config.directions; i++)
            {
                float dot = Vector3.Dot(directionToObstacle, _directionVectors[i]);
                if (dot > 0)
                {
                    float danger = dot * dangerWeight * config.avoidanceWeight;
                    _dangerArray[i] = Mathf.Max(_dangerArray[i], danger);
                }
            }
        }

        private void CalculateInterest(Vector3 position)
        {
            Vector3 toTarget = position - transform.position;
            toTarget.y = 0;

            if (toTarget.magnitude < 0.1f) return;

            Vector3 directionToTarget = toTarget.normalized;

            for (int i = 0; i < config.directions; i++)
            {
                float dot = Vector3.Dot(directionToTarget, _directionVectors[i]);
                _interestArray[i] = Mathf.Max(0, dot * config.seekWeight);
            }
        }

        private Vector3 CalculateResultDirection()
        {
            Vector3 resultDirection = Vector3.zero;
            float maxValue = float.MinValue;

            for (int i = 0; i < config.directions; i++)
            {
                float value = _interestArray[i] - _dangerArray[i];

                if (value > maxValue)
                {
                    maxValue = value;
                    resultDirection = _directionVectors[i];
                }
            }

            _resultDirection = resultDirection;
            if (_resultDirection != Vector3.zero)
            {
                _lastNonZeroDirection = _resultDirection;
            }

            var direction = _resultDirection != Vector3.zero ? _resultDirection : _lastNonZeroDirection;
            return _resultDirection;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;

            // Draw direction vectors with color intensity based on interest/danger
            for (int i = 0; i < config.directions; i++)
            {
                float interest = _interestArray[i];
                float danger = _dangerArray[i];

                // Green for interest, Red for danger
                Gizmos.color = new Color(danger, interest, 0, 0.5f);
                Gizmos.DrawRay(transform.position, _directionVectors[i] * config.dangerDecayDistance);
            }

            // Draw result direction
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, _resultDirection * config.dangerDecayDistance * 1.2f);

            // Use the provided DrawGizmos from OverlapNonAlloc
            config.obstacleDetector?.DrawGizmos(Color.yellow, Color.red);
        }
#endif
    }
}